using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models;
using QuanLyBanHangDoAnThucUong.Helpers;
using QuanLyBanHangDoAnThucUong.Models.Entities;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public class HomeController : BaseClientController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : base(context)
        {
            _logger = logger;
        }



        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 12;
            var gianHangs = _context.GianHangs.AsQueryable();

            var totalCount = await gianHangs.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var paginatedItems = await gianHangs
                .OrderByDescending(g => g.MaDoiTac)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gợi ý thông minh
            var recommendedFoods = await GetRecommendedFoods();
            ViewBag.RecommendedFoods = recommendedFoods;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedItems);
        }

        private async Task<List<MonAn>> GetRecommendedFoods()
        {
            var maKH = GetCurrentCustomerId();
            var query = _context.MonAns
                .Include(m => m.GianHang)
                .Include(m => m.BienTheMonAns)
                .AsQueryable();

            if (maKH != null)
            {
                // 1. Lấy từ khóa tìm kiếm gần đây
                var lastSearch = await _context.LichSuTimKiems
                    .Where(l => l.MaKH == maKH)
                    .OrderByDescending(l => l.NgayTimKiem)
                    .Select(l => l.TuKhoa)
                    .Take(5)
                    .ToListAsync();

                // 2. Lấy các món đã từng đặt
                var orderedMonAnIds = await _context.ChiTietDonHangs
                    .Include(ct => ct.DonHang)
                    .Where(ct => ct.DonHang!.MaKH == maKH)
                    .Select(ct => ct.BienTheMonAn!.MaMonAn)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();

                // Logic gợi ý: Ưu tiên theo lịch sử
                var results = await query
                    .Where(m => orderedMonAnIds.Contains(m.MaMonAn))
                    .OrderByDescending(m => m.SoLuotBan)
                    .Take(10)
                    .ToListAsync();

                if (results.Count < 10 && lastSearch.Any())
                {
                    // Thêm các món dựa trên từ khóa tìm kiếm gần đây
                    foreach (var s in lastSearch)
                    {
                        if (string.IsNullOrEmpty(s)) continue;
                        if (results.Count >= 10) break;

                        var matchKeyword = await query
                            .Where(m => !results.Select(r => r.MaMonAn).Contains(m.MaMonAn))
                            .Where(m => (m.TenMon != null && m.TenMon.Contains(s)) || (m.ThanhPhan != null && m.ThanhPhan.Contains(s)))
                            .OrderByDescending(m => m.SoLuotBan)
                            .Take(10 - results.Count)
                            .ToListAsync();

                        results.AddRange(matchKeyword);
                    }
                }

                if (results.Count >= 6) return results;

                // Nếu vẫn chưa đủ, lấy thêm các món bán chạy nhất (Popularity)
                var popular = await query
                    .Where(m => !results.Select(r => r.MaMonAn).Contains(m.MaMonAn))
                    .OrderByDescending(m => m.SoLuotBan)
                    .Take(10 - results.Count)
                    .ToListAsync();
                
                results.AddRange(popular);
                return results;
            }

            // Nếu chưa đăng nhập, chỉ lấy món bán chạy nhất
            return await query
                .OrderByDescending(m => m.SoLuotBan)
                .Take(10)
                .ToListAsync();
        }

        [Route("filter")]
        public async Task<IActionResult> Filter(string searchString, List<string> cities, List<string> wards, string sortBy, string order, int page = 1)
        {
            const int pageSize = 12;
            var query = _context.GianHangs
                .Include(g => g.MonAns)
                .AsQueryable();

            // Lọc theo từ khóa (Tên quán, món ăn, thành phần...)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(g => 
                    (g.TenGianHang != null && g.TenGianHang.Contains(searchString)) || 
                    g.MonAns.Any(m => (m.TenMon != null && m.TenMon.Contains(searchString)) || (m.ThanhPhan != null && m.ThanhPhan.Contains(searchString)))
                );

                // Lưu lịch sử tìm kiếm nếu đã đăng nhập
                var maKH = GetCurrentCustomerId();
                if (maKH != null)
                {
                    var history = new LichSuTimKiem
                    {
                        MaKH = maKH.Value,
                        TuKhoa = searchString,
                        NgayTimKiem = DateTime.Now
                    };
                    _context.LichSuTimKiems.Add(history);
                    await _context.SaveChangesAsync();
                }
            }

            // Lọc theo thành phố
            if (cities != null && cities.Any())
            {
                query = query.Where(g => cities.Contains(g.ThanhPho));
            }
            // Lọc theo phường/xã
            if (wards != null && wards.Any())
            {
                query = query.Where(g => wards.Contains(g.PhuongXa));
            }

            // Sắp xếp
            switch (sortBy)
            {
                case "ctime": // Mới nhất
                    query = query.OrderByDescending(g => g.MaDoiTac);
                    break;
                case "sales": // Doanh số bán hàng đầu
                    query = query.OrderByDescending(g => g.MonAns.Sum(m => m.SoLuotBan ?? 0));
                    break;
                case "price": // Giá
                    if (order == "desc")
                        query = query.OrderByDescending(g => g.MonAns.SelectMany(m => m.BienTheMonAns).Any() ? g.MonAns.SelectMany(m => m.BienTheMonAns).Min(b => b.GiaBan) : 0);
                    else
                        query = query.OrderBy(g => g.MonAns.SelectMany(m => m.BienTheMonAns).Any() ? g.MonAns.SelectMany(m => m.BienTheMonAns).Min(b => b.GiaBan) : 0);
                    break;
                default: // Liên quan (Mặc định - Dựa trên lượt xem)
                    query = query.OrderByDescending(g => g.LuotXem);
                    break;
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var results = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var rawCities = await _context.GianHangs.Where(g => !string.IsNullOrEmpty(g.ThanhPho)).Select(g => g.ThanhPho).ToListAsync();
            var normalizedCities = rawCities
                .Select(c => c.Trim())
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            ViewBag.CitiesList = normalizedCities;
            ViewBag.SelectedCities = cities;

            var wardsQuery = _context.GianHangs.Where(g => !string.IsNullOrEmpty(g.PhuongXa));
            if (cities != null && cities.Any())
            {
                wardsQuery = wardsQuery.Where(g => cities.Contains(g.ThanhPho));
            }
            var rawWards = await wardsQuery.Select(g => g.PhuongXa).ToListAsync();
            var normalizedWards = rawWards
                .Select(w => w.Trim())
                .Distinct()
                .OrderBy(w => w)
                .ToList();
            ViewBag.WardsList = normalizedWards;
            ViewBag.SelectedWards = wards;

            ViewBag.SearchString = searchString;
            ViewBag.SortBy = sortBy;
            ViewBag.Order = order;
            ViewBag.CurrentSearch = searchString;
            ViewBag.SortBy = sortBy;
            ViewBag.Order = order;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Gợi ý thông minh cho sidebar
            ViewBag.RecommendedFoods = await GetRecommendedFoods();

            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> LiveSearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return Json(new List<object>());

            var kw = keyword.ToLower();

            var results = await _context.MonAns
                .Include(m => m.GianHang)
                .Include(m => m.BienTheMonAns)
                .Where(m => (m.TenMon != null && m.TenMon.ToLower().Contains(kw)) || 
                            (m.ThanhPhan != null && m.ThanhPhan.ToLower().Contains(kw)))
                .OrderByDescending(m => m.SoLuotBan)
                .Take(5)
                .Select(m => new
                {
                    maMonAn = m.MaMonAn,
                    tenMon = m.TenMon,
                    tenGianHang = m.GianHang != null ? m.GianHang.TenGianHang : "",
                    giaBan = m.BienTheMonAns.Any() ? m.BienTheMonAns.Min(b => b.GiaBan) : 0,
                    hinhAnh = m.BienTheMonAns.FirstOrDefault() != null ? m.BienTheMonAns.FirstOrDefault().HinhAnhMonAn : "default_food.jpg",
                    slug = SlugHelper.GenerateSlugOptimal(m.TenMon ?? "mon-an")
                })
                .ToListAsync();

            return Json(results);
        }

        public async Task<IActionResult> StoreDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var gianHang = await _context.GianHangs
                .Include(g => g.DoiTac)
                .Include(g => g.MonAns)
                    .ThenInclude(m => m.BienTheMonAns)
                .Include(g => g.MonAns)
                    .ThenInclude(m => m.TheLoaiMonAn)
                .FirstOrDefaultAsync(m => m.MaGianHang == id);

            if (gianHang == null)
            {
                return NotFound();
            }

            // Tăng lượt xem khi khách vào xem quán
            gianHang.LuotXem++;
            _context.Update(gianHang);
            await _context.SaveChangesAsync();

            // Group the dishes by category to make it easier for the view to render ShopeeFood style
            var menuItems = gianHang.MonAns
                .DistinctBy(m => m.MaMonAn)
                .Where(m => m.BienTheMonAns != null && m.BienTheMonAns.Any()) // Only show dishes that have variants/prices
                .GroupBy(m => m.TheLoaiMonAn)
                .OrderBy(g => g.Key?.TenLoai)
                .ToList();

            ViewBag.MenuItems = menuItems;

            // Get category IDs of the current store
            var categoryIds = gianHang.MonAns
                .Select(m => m.MaLoaiMon)
                .Distinct()
                .ToList();

            // Lấy danh sách gian hàng gợi ý thông minh
            var maKH = GetCurrentCustomerId();
            var similarStoresCandidates = await _context.GianHangs
                .Include(g => g.MonAns)
                .Where(g => g.MaDoiTac != id && g.TrangThaiGianHang == "Mở cửa")
                // Lấy các quán cùng thành phố hoặc cùng loại món để lọc bớt
                .Where(g => g.ThanhPho == gianHang.ThanhPho || g.MonAns.Any(m => categoryIds.Contains(m.MaLoaiMon)))
                .Take(20) 
                .ToListAsync();

            List<GianHang> similarStores;
            if (maKH != null)
            {
                var lastSearch = await _context.LichSuTimKiems
                    .Where(l => l.MaKH == maKH)
                    .OrderByDescending(l => l.NgayTimKiem)
                    .Select(l => l.TuKhoa)
                    .Take(3)
                    .ToListAsync();

                similarStores = similarStoresCandidates
                    .OrderByDescending(g => g.ThanhPho == gianHang.ThanhPho)
                    .ThenByDescending(g => g.MonAns.Any(m => categoryIds.Contains(m.MaLoaiMon)))
                    .ThenByDescending(g => lastSearch.Any(s => !string.IsNullOrEmpty(s) && (g.TenGianHang!.Contains(s) || g.MonAns.Any(m => m.TenMon!.Contains(s)))))
                    .ThenByDescending(g => g.LuotXem)
                    .Take(5)
                    .ToList();
            }
            else
            {
                similarStores = similarStoresCandidates
                    .OrderByDescending(g => g.ThanhPho == gianHang.ThanhPho)
                    .ThenByDescending(g => g.LuotXem)
                    .Take(5)
                    .ToList();
            }

            ViewBag.SimilarStores = similarStores;

            return View(gianHang);
        }

        public async Task<IActionResult> MonAnDetails(string slug, int? id)
        {
            if (id == null) return NotFound();

            var monAn = await _context.MonAns
                .Include(m => m.GianHang)
                .Include(m => m.TheLoaiMonAn)
                .Include(m => m.BienTheMonAns)
                .FirstOrDefaultAsync(m => m.MaMonAn == id);

            if (monAn == null) return NotFound();

            // Redirect to SEO URL if accessed via /Home/MonAnDetails/id
            var expectedSlug = SlugHelper.GenerateSlugOptimal(monAn.TenMon ?? "mon-an");
            if (string.IsNullOrEmpty(slug) || slug != expectedSlug)
            {
                return RedirectToRoutePermanent("monAnSEO", new { slug = expectedSlug, id = id });
            }

            // Gợi ý món ăn tương tự theo thuật toán (không chỉ dựa vào thể loại)
            string tenMonLower = monAn.TenMon?.ToLower() ?? "";
            string thanhPhanLower = monAn.ThanhPhan?.ToLower() ?? "";
            bool isChay = tenMonLower.Contains("chay") || thanhPhanLower.Contains("chay");
            
            var keywords = tenMonLower.Split(new[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(k => k.Length > 2 && k != "của" && k != "và" && k != "với" && k != "chay" && k != "mặn")
                .ToList();

            var candidatesList = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .Include(m => m.GianHang)
                .Where(m => m.MaMonAn != id && m.BienTheMonAns.Any())
                .ToListAsync();

            var similarItems = candidatesList
                .Where(m => 
                {
                    string mTen = m.TenMon?.ToLower() ?? "";
                    string mThanh = m.ThanhPhan?.ToLower() ?? "";
                    bool mIsChay = mTen.Contains("chay") || mThanh.Contains("chay");
                    
                    // Lọc nghiêm ngặt: Món chay gợi ý chay, món mặn gợi ý mặn
                    if (isChay != mIsChay) return false;
                    
                    // Phải cùng loại món HOẶC có chứa từ khóa của món đang xem
                    return m.MaLoaiMon == monAn.MaLoaiMon || keywords.Any(kw => mTen.Contains(kw));
                })
                .OrderByDescending(m => 
                {
                    int score = 0;
                    string mTen = m.TenMon?.ToLower() ?? "";
                    
                    if (m.MaLoaiMon == monAn.MaLoaiMon) score += 10;
                    
                    foreach (var kw in keywords)
                    {
                        if (mTen.Contains(kw)) score += 5;
                    }
                    
                    return score;
                })
                .ThenByDescending(m => m.SoLuotBan)
                .Take(8)
                .ToList();

            ViewBag.SimilarItems = similarItems;

            // Vẫn giữ RelatedItems (Món khác cùng quán)
            var relatedItems = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .Where(m => m.MaGianHang == monAn.MaGianHang && m.MaMonAn != id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedItems = relatedItems;

            // Kiểm tra quyền đánh giá (Chỉ khách hàng đã mua món và đơn hàng "Hoàn thành" mới được đánh giá)
            bool canReview = false;
            int? availableOrderId = null;
            var maKH = GetCurrentCustomerId();
            if (maKH != null)
            {
                var eligibleOrders = await _context.DonHangs
                    .Where(d => d.MaKH == maKH && 
                                (d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao") && 
                                d.ChiTietDonHangs.Any(c => c.BienTheMonAn.MaMonAn == id))
                    .Select(d => d.MaDonHang)
                    .ToListAsync();

                var reviewedOrders = await _context.DanhGiaMonAns
                    .Where(dg => dg.MaKH == maKH && dg.MaMonAn == id && dg.MaDonHang != null)
                    .Select(dg => dg.MaDonHang.Value)
                    .ToListAsync();

                var unreviewedOrders = eligibleOrders.Except(reviewedOrders).ToList();
                canReview = unreviewedOrders.Any();
                availableOrderId = unreviewedOrders.FirstOrDefault();

                if (!canReview)
                {
                    var purchaseCount = eligibleOrders.Count;
                    var totalReviews = await _context.DanhGiaMonAns.CountAsync(d => d.MaKH == maKH && d.MaMonAn == id);
                    if (purchaseCount > totalReviews)
                    {
                        canReview = true;
                    }
                }
            }
            ViewBag.CanReview = canReview;
            ViewBag.AvailableOrderId = availableOrderId;

            return View(monAn);
        }

        // ===== API ĐÁNH GIÁ =====
        [HttpGet]
        
        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) 
                return Json(new { monAn = new List<object>(), gianHang = new List<object>() });

            var queryStr = q.ToLower();

            var monAn = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .Where(m => m.TenMon.ToLower().Contains(queryStr))
                .OrderByDescending(m => m.SoLuotDanhGia)
                .Take(5)
                .Select(m => new {
                    id = m.MaMonAn,
                    ten = m.TenMon,
                    hinh = m.BienTheMonAns.FirstOrDefault() != null ? m.BienTheMonAns.FirstOrDefault().HinhAnhMonAn : null,
                    gia = m.BienTheMonAns.FirstOrDefault() != null ? m.BienTheMonAns.FirstOrDefault().GiaBan : 0
                })
                .ToListAsync();

            var gianHang = await _context.GianHangs
                .Where(g => g.TenGianHang.ToLower().Contains(queryStr))
                .OrderByDescending(g => g.DanhGiaGianHang)
                .Take(3)
                .Select(g => new {
                    id = g.MaGianHang,
                    ten = g.TenGianHang,
                    hinh = g.HinhAnh,
                    diaChi = g.DiaChiCuThe
                })
                .ToListAsync();

            return Json(new { monAn, gianHang });
        }

        public async Task<IActionResult> GetDanhGia(int maMonAn, int page = 1, string sort = "newest", int? filterStar = null)
        {
            var maKH = HttpContext.Session.GetInt32("MaKH");
            const int pageSize = 5;

            var query = _context.DanhGiaMonAns
                .Include(d => d.KhachHang)
                .Where(d => d.MaMonAn == maMonAn);

            if (filterStar.HasValue && filterStar > 0)
                query = query.Where(d => d.SoSao == filterStar);

            query = sort switch
            {
                "oldest" => query.OrderBy(d => d.NgayDanhGia),
                "highest" => query.OrderByDescending(d => d.SoSao),
                "lowest" => query.OrderBy(d => d.SoSao),
                _ => query.OrderByDescending(d => d.NgayDanhGia)
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var dbReviews = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    d.MaDanhGia,
                    d.MaKH,
                    d.SoSao,
                    d.NoiDung,
                    d.NgayDanhGia,
                    TenKH = d.KhachHang != null ? d.KhachHang.TenKH : "Ẩn danh",
                    Avatar = d.KhachHang != null ? d.KhachHang.Avatar : null,
                    d.PhanHoiCuaDoiTac,
                    d.NgayPhanHoi
                })
                .ToListAsync();

            var reviews = dbReviews.Select(d => new
            {
                d.MaDanhGia,
                d.MaKH,
                d.SoSao,
                d.NoiDung,
                NgayDanhGia = d.NgayDanhGia.ToString("dd/MM/yyyy HH:mm"),
                d.TenKH,
                d.Avatar,
                d.PhanHoiCuaDoiTac,
                IsOwner = (maKH != null && d.MaKH == maKH),
                NgayPhanHoi = d.NgayPhanHoi.HasValue ? d.NgayPhanHoi.Value.ToString("dd/MM/yyyy HH:mm") : null
            }).ToList();

            // Tổng hợp số sao (tính trên TẤT CẢ đánh giá của món ăn, không bị ảnh hưởng bởi filterStar)
            var starSummary = await _context.DanhGiaMonAns
                .Where(d => d.MaMonAn == maMonAn)
                .GroupBy(d => d.SoSao)
                .Select(g => new { Star = g.Key, Count = g.Count() })
                .ToListAsync();
            
            // Total reviews count for the item (unfiltered)
            var totalUnfilteredCount = await _context.DanhGiaMonAns.CountAsync(d => d.MaMonAn == maMonAn);

            return Json(new
            {
                reviews,
                currentPage = page,
                totalPages,
                totalCount, // filtered count
                totalUnfilteredCount, // unfiltered count
                starSummary
            });
        }

        [HttpPost]
        public async Task<IActionResult> XoaDanhGia(int id)
        {
            var maKH = GetCurrentCustomerId();
            if (maKH == null) return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var danhGia = await _context.DanhGiaMonAns.FirstOrDefaultAsync(d => d.MaDanhGia == id && d.MaKH == maKH);
            if (danhGia == null) return Json(new { success = false, message = "Không tìm thấy đánh giá hoặc không có quyền xóa" });

            _context.DanhGiaMonAns.Remove(danhGia);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã xóa đánh giá" });
        }
        [HttpPost]
        public async Task<IActionResult> ThemDanhGia(int maMonAn, int soSao, string? noiDung, int? maDonHang)
        {
            var maKH = HttpContext.Session.GetInt32("MaKH");
            if (maKH == null)
            {
                // Try finding from MaTaiKhoan
                var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
                var vaiTro = HttpContext.Session.GetString("VaiTro");
                if (maTaiKhoan != null && vaiTro == "KhachHang")
                {
                    var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan);
                    if (kh != null) maKH = kh.MaKH;
                }
            }

            if (maKH == null)
                return Json(new { success = false, requireLogin = true, message = "Vui lòng đăng nhập để đánh giá." });

            try
            {
                var danhGia = new Models.Entities.DanhGiaMonAn
                {
                    MaMonAn = maMonAn,
                    MaKH = maKH,
                    MaDonHang = maDonHang,
                    SoSao = soSao,
                    NoiDung = noiDung,
                    NgayDanhGia = DateTime.Now
                };

                _context.DanhGiaMonAns.Add(danhGia);
                await _context.SaveChangesAsync();
                
                // (Trigger trg_UpdateDanhGiaMonAn trong SQL Server sẽ tự động tính lại số sao cho MonAn và GianHang)
                
                return Json(new { success = true, message = "Đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("errors.txt", ex.ToString());
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}








