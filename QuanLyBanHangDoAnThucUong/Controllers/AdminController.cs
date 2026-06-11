using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models.Entities;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== KIỂM TRA QUYỀN ADMIN =====
        private bool CheckIsAdmin()
        {
            var vaiTro = HttpContext.Session.GetString("VaiTro");
            return vaiTro == "Admin";
        }

        // ===== DASHBOARD ADMIN =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            var dashboard = new
            {
                TongDoiTac = await _context.DoiTacs.CountAsync(),
                TongGianHang = await _context.GianHangs.CountAsync(),
                TongMonAn = await _context.MonAns.CountAsync(),
                TongKhachHang = await _context.KhachHangs.CountAsync(),
                TongDonHang = await _context.DonHangs.CountAsync(), // Fixed
                TongTaiKhoan = await _context.TaiKhoans.CountAsync(),
                TongLoaiMon = await _context.TheLoaiMonAns.CountAsync(),
                DoiTacHoatDong = await _context.DoiTacs
                    .CountAsync(d => d.TrangThaiDoiTac == "Còn đang hoạt động"),
                GianHangMoCua = await _context.GianHangs
                    .CountAsync(g => g.TrangThaiGianHang == "Mở cửa")
            };

            var recentPartners = await _context.DoiTacs
                .Include(d => d.TaiKhoan)
                .OrderByDescending(d => d.MaDoiTac)
                .Take(5)
                .ToListAsync();

            // --- TÍNH TOÁN DOANH THU TOÀN HỆ THỐNG ---
            var completedOrders = await _context.DonHangs
                .Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao")
                .ToListAsync();

            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            ViewBag.TongDoanhThu = completedOrders.Sum(d => d.ThanhTienKhachTra);
            ViewBag.DoanhThuThangNay = completedOrders.Where(d => d.NgayTaoDon >= startOfMonth).Sum(d => d.ThanhTienKhachTra);
            ViewBag.DoanhThuHomNay = completedOrders.Where(d => d.NgayTaoDon.Date == today).Sum(d => d.ThanhTienKhachTra);

            // Chart data for last 7 days
            var last7DaysRevenues = new List<decimal>();
            var last7DaysLabels = new List<string>();
            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                var revenue = completedOrders
                    .Where(d => d.NgayTaoDon.Date == date)
                    .Sum(d => d.ThanhTienKhachTra);
                last7DaysRevenues.Add(revenue);
                last7DaysLabels.Add(date.ToString("dd/MM"));
            }
            ViewBag.Last7DaysRevenues = last7DaysRevenues;
            ViewBag.Last7DaysLabels = last7DaysLabels;

            ViewBag.Dashboard = dashboard;
            ViewBag.RecentPartners = recentPartners;
            return View();
        }

        // ===== API LẤY DỮ LIỆU BIỂU ĐỒ ADMIN =====
        [HttpGet]
        public async Task<IActionResult> GetAdminChartData(string filter)
        {
            if (!CheckIsAdmin()) return Unauthorized();

            var completedOrders = await _context.DonHangs
                .Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao")
                .ToListAsync();

            var today = DateTime.Today;
            var labels = new List<string>();
            var data = new List<decimal>();
            string title = "";

            if (filter == "daily")
            {
                // Today's hours
                title = "Biểu Đồ Doanh Thu Hôm Nay";
                for (int i = 8; i <= 22; i += 2) // 8h to 22h
                {
                    labels.Add($"{i}h");
                    var rev = completedOrders
                        .Where(d => d.NgayTaoDon.Date == today && d.NgayTaoDon.Hour >= i && d.NgayTaoDon.Hour < i + 2)
                        .Sum(d => d.ThanhTienKhachTra);
                    data.Add(rev);
                }
            }
            else if (filter == "monthly")
            {
                title = $"Biểu Đồ Doanh Thu Tháng {today.Month}";
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                var interval = daysInMonth / 6; // Create about 6-7 points

                for (int i = 1; i <= daysInMonth; i += interval)
                {
                    var d = startOfMonth.AddDays(i - 1);
                    var endD = d.AddDays(interval - 1);
                    if (endD.Month != today.Month) endD = new DateTime(today.Year, today.Month, daysInMonth);
                    
                    labels.Add(d.Day == endD.Day ? $"{d.Day}/{d.Month}" : $"{d.Day}-{endD.Day}/{d.Month}");
                    var rev = completedOrders
                        .Where(o => o.NgayTaoDon.Date >= d && o.NgayTaoDon.Date <= endD)
                        .Sum(o => o.ThanhTienKhachTra);
                    data.Add(rev);
                }
            }
            else // 'total' or 'yearly'
            {
                title = "Biểu Đồ Doanh Thu";
                for (int i = 1; i <= 12; i++)
                {
                    labels.Add($"T{i}");
                    data.Add(completedOrders.Where(o => o.NgayTaoDon.Year == today.Year && o.NgayTaoDon.Month == i).Sum(o => o.ThanhTienKhachTra));
                }
            }

            return Json(new { success = true, labels, data, title });
        }

        // ===== QUẢN LÝ ĐỐI TÁC =====
        [HttpGet]
        public async Task<IActionResult> QuanLyDoiTac(int page = 1, string searchStr = null)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var query = _context.DoiTacs.Include(d => d.TaiKhoan).AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(d => d.TenQuanDoiTac != null && d.TenQuanDoiTac.Contains(searchStr));
            }

            var doiTacs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.SearchStr = searchStr;

            return View(doiTacs);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoiTacDetails(int id)
        {
            if (!CheckIsAdmin()) return Json(new { success = false, message = "Unauthorized" });

            var doiTac = await _context.DoiTacs
                .Include(d => d.TaiKhoan)
                .FirstOrDefaultAsync(d => d.MaDoiTac == id);

            if (doiTac == null) return Json(new { success = false, message = "Not found" });

            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == id);
            
            var orders = new List<DonHang>();
            if (gianHang != null)
            {
                orders = await _context.DonHangs
                    .Where(d => d.MaGianHang == gianHang.MaGianHang && d.TrangThaiDonHang == "Hoàn thành" && d.TrangThaiThanhToan == "Đã thanh toán")
                    .ToListAsync();
            }

            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var dailyRev = orders.Where(d => d.NgayTaoDon.Date == today).Sum(d => d.ThanhTienKhachTra);
            var monthlyRev = orders.Where(d => d.NgayTaoDon >= startOfMonth).Sum(d => d.ThanhTienKhachTra);
            var yearlyRev = orders.Where(d => d.NgayTaoDon >= startOfYear).Sum(d => d.ThanhTienKhachTra);
            var totalRev = orders.Sum(d => d.ThanhTienKhachTra);

            // Dữ liệu biểu đồ doanh thu theo năm
            var labels = new List<string>();
            var data = new List<decimal>();
            for (int i = 1; i <= 12; i++)
            {
                labels.Add($"T{i}");
                data.Add(orders.Where(o => o.NgayTaoDon.Year == today.Year && o.NgayTaoDon.Month == i).Sum(o => o.ThanhTienKhachTra));
            }

            return Json(new {
                success = true,
                partnerInfo = new {
                    ten = doiTac.TenQuanDoiTac,
                    sdt = doiTac.SoDTDoiTac,
                    email = doiTac.EmailDTac,
                    diaChi = doiTac.DiaChiDoiTac,
                    trangThai = doiTac.TrangThaiDoiTac,
                    taiKhoan = doiTac.TaiKhoan?.TenDangNhap
                },
                storeInfo = gianHang != null ? new {
                    ten = gianHang.TenGianHang,
                    diaChi = gianHang.DiaChiCuThe,
                    moCua = gianHang.GioMo.ToString(@"hh\:mm"),
                    dongCua = gianHang.GioDong.ToString(@"hh\:mm"),
                    danhGia = gianHang.DanhGiaTB,
                    trangThai = gianHang.TrangThaiGianHang
                } : null,
                revenue = new {
                    daily = dailyRev,
                    monthly = monthlyRev,
                    yearly = yearlyRev,
                    total = totalRev
                },
                chartData = new { labels, data }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetDoiTacChartData(int maDoiTac, string filter)
        {
            if (!CheckIsAdmin()) return Unauthorized();

            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == maDoiTac);
            if (gianHang == null) return Json(new { success = false, message = "Not found" });

            var orders = await _context.DonHangs
                .Where(d => d.MaGianHang == gianHang.MaGianHang && (d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao"))
                .ToListAsync();

            var today = DateTime.Today;
            var labels = new List<string>();
            var data = new List<decimal>();

            if (filter == "daily")
            {
                for (int i = 8; i <= 22; i += 2)
                {
                    labels.Add($"{i}h");
                    data.Add(orders.Where(o => o.NgayTaoDon.Date == today && o.NgayTaoDon.Hour >= i && o.NgayTaoDon.Hour < i + 2).Sum(o => o.ThanhTienKhachTra));
                }
            }
            else if (filter == "monthly")
            {
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                var interval = daysInMonth / 6;

                for (int i = 1; i <= daysInMonth; i += interval)
                {
                    var d = startOfMonth.AddDays(i - 1);
                    var endD = d.AddDays(interval - 1);
                    if (endD.Month != today.Month) endD = new DateTime(today.Year, today.Month, daysInMonth);
                    
                    labels.Add(d.Day == endD.Day ? $"{d.Day}/{d.Month}" : $"{d.Day}-{endD.Day}/{d.Month}");
                    data.Add(orders.Where(o => o.NgayTaoDon.Date >= d && o.NgayTaoDon.Date <= endD).Sum(o => o.ThanhTienKhachTra));
                }
            }
            else if (filter == "yearly")
            {
                for (int i = 1; i <= 12; i++)
                {
                    labels.Add($"T{i}");
                    data.Add(orders.Where(o => o.NgayTaoDon.Year == today.Year && o.NgayTaoDon.Month == i).Sum(o => o.ThanhTienKhachTra));
                }
            }
            else // 'total'
            {
                for (int i = 1; i <= 12; i++)
                {
                    labels.Add($"T{i}");
                    data.Add(orders.Where(o => o.NgayTaoDon.Year == today.Year && o.NgayTaoDon.Month == i).Sum(o => o.ThanhTienKhachTra));
                }
            }

            return Json(new { success = true, labels, data });
        }

        [HttpGet]
        public async Task<IActionResult> GetDoiTacOrders(int maDoiTac, int page = 1, string searchStr = null, string tuNgay = null, string denNgay = null, string trangThai = null, string sortOrder = "newest")
        {
            if (!CheckIsAdmin()) return Json(new { success = false, message = "Unauthorized" });

            const int pageSize = 5;
            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == maDoiTac);
            if (gianHang == null) return Json(new { success = true, orders = new List<object>(), totalPages = 0 });

            var query = _context.DonHangs
                .Include(d => d.KhachHang)
                .Where(d => d.MaGianHang == gianHang.MaGianHang);

            // Tìm kiếm theo Mã Đơn hoặc Tên Khách Hàng
            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(d => d.MaDonHang.ToString().Contains(searchStr) || 
                                        (d.KhachHang != null && d.KhachHang.TenKH.Contains(searchStr)));
            }

            // Lọc theo ngày
            if (!string.IsNullOrEmpty(tuNgay) && DateTime.TryParse(tuNgay, out DateTime dtTuNgay))
            {
                query = query.Where(d => d.NgayTaoDon.Date >= dtTuNgay.Date);
            }
            if (!string.IsNullOrEmpty(denNgay) && DateTime.TryParse(denNgay, out DateTime dtDenNgay))
            {
                query = query.Where(d => d.NgayTaoDon.Date <= dtDenNgay.Date);
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.TrangThaiDonHang == trangThai);
            }

            // Sắp xếp
            if (sortOrder == "price_asc")
            {
                query = query.OrderBy(d => d.ThanhTienKhachTra);
            }
            else if (sortOrder == "price_desc")
            {
                query = query.OrderByDescending(d => d.ThanhTienKhachTra);
            }
            else // newest
            {
                query = query.OrderByDescending(d => d.NgayTaoDon);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new {
                    maDonHang = d.MaDonHang,
                    khachHang = d.KhachHang != null ? d.KhachHang.TenKH : "Khách vãng lai",
                    ngayTao = d.NgayTaoDon.ToString("dd/MM/yyyy HH:mm"),
                    tongTien = d.ThanhTienKhachTra,
                    trangThai = d.TrangThaiDonHang
                })
                .ToListAsync();

            return Json(new {
                success = true,
                orders = orders,
                totalPages = totalPages,
                currentPage = page
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            if (!CheckIsAdmin()) return Json(new { success = false, message = "Unauthorized" });

            var order = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(d => d.MaDonHang == id);

            if (order == null) return Json(new { success = false });

            var details = await _context.ChiTietDonHangs
                .Include(c => c.BienTheMonAn)
                    .ThenInclude(b => b.MonAn)
                .Where(c => c.MaDonHang == id)
                .Select(c => new {
                    tenMon = c.BienTheMonAn.MonAn.TenMon,
                    moTa = c.BienTheMonAn.MoTaMonAn,
                    hinhAnh = c.BienTheMonAn.HinhAnhMonAn,
                    soLuong = c.SoLuongMua,
                    giaBan = c.GiaBan,
                    thanhTien = c.SoLuongMua * c.GiaBan
                })
                .ToListAsync();

            return Json(new {
                success = true,
                orderInfo = new {
                    maDonHang = order.MaDonHang,
                    ngayTao = order.NgayTaoDon.ToString("dd/MM/yyyy HH:mm"),
                    khachHang = order.KhachHang != null ? order.KhachHang.TenKH : "Khách vãng lai",
                    soDienThoai = order.KhachHang != null ? order.KhachHang.SoDTKH : "N/A",
                    tongTien = order.TongTienMon,
                    phiGiaoHang = order.PhiShip,
                    tienGiam = order.GiamGia,
                    thanhTienKhachTra = order.ThanhTienKhachTra,
                    trangThai = order.TrangThaiDonHang,
                    thanhToan = order.TrangThaiThanhToan,
                    phuongThucThanhToan = order.PhuongThucThanhToan
                },
                items = details
            });
        }

        [HttpGet]
        public async Task<IActionResult> DanhGiaGianHang(int page = 1, string searchStr = null)
        {
            if (!CheckIsAdmin()) return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var query = _context.GianHangs.Include(g => g.DoiTac).AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(g => g.TenGianHang != null && g.TenGianHang.Contains(searchStr));
            }
 
            var totalCount = await query.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            var gianHangs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Tot = gianHangs.Where(g => g.DanhGiaTB >= 4).ToList();
            ViewBag.TrungBinh = gianHangs.Where(g => g.DanhGiaTB >= 3 && g.DanhGiaTB < 4).ToList();
            ViewBag.Kem = gianHangs.Where(g => g.DanhGiaTB < 3).ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchStr = searchStr;

            return View();
        }

        // ===== QUẢN LÝ GIAN HÀNG (ĐÃ BỎ LINK TRONG SIDEBAR NHƯNG GIỮ LẠI CHO SAFE) =====
        [HttpGet]
        public async Task<IActionResult> QuanLyGianHang(int page = 1)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var gianHangs = await _context.GianHangs
                .Include(g => g.DoiTac)
                .Include(g => g.DieuLe)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.GianHangs.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(gianHangs);
        }

        // ===== QUẢN LÝ KHÁCH HÀNG =====
        [HttpGet]
        public async Task<IActionResult> QuanLyKhachHang(int page = 1, string searchStr = null)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var query = _context.KhachHangs.AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(k => k.TenKH.Contains(searchStr) || k.SoDTKH.Contains(searchStr));
            }

            var khachHangs = await query 
                .Skip((page - 1) * pageSize) // Phân trang. Skip dung de bo qua cac ban ghi cua cac trang truoc, Take dung de lay so ban ghi cua trang hien tai
                .Take(pageSize)// Lay so ban ghi cua trang hien tai
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.SearchStr = searchStr;

            return View(khachHangs);
        }

        [HttpGet]
        public async Task<IActionResult> GetKhachHangDetails(int id)
        {
            if (!CheckIsAdmin()) return Json(new { success = false });

            var kh = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKH == id);

            if (kh == null) return Json(new { success = false });

            var totalOrders = await _context.DonHangs.CountAsync(d => d.MaKH == id);
            var totalSpent = await _context.DonHangs.Where(d => d.MaKH == id && d.TrangThaiDonHang == "Hoàn thành").SumAsync(d => d.ThanhTienKhachTra);

            return Json(new {
                success = true,
                info = new {
                    ten = kh.TenKH,
                    sdt = kh.SoDTKH,
                    email = kh.EmailKH,
                    diaChi = kh.DiaChiCuThe,
                    ngaySinh = "Không có", // Khách hàng model không lưu ngày sinh
                    ngayDangKy = kh.NgayDangKy.ToString("dd/MM/yyyy"),
                    gioiTinh = kh.GioiTinhKH,
                    taiKhoan = kh.TaiKhoan?.TenDangNhap
                },
                stats = new {
                    orders = totalOrders,
                    spent = totalSpent
                }
            });
        }

        // ===== QUẢN LÝ TÀI KHOẢN =====
        [HttpGet]
        public async Task<IActionResult> QuanLyTaiKhoan(int page = 1, string searchStr = null, string role = "KhachHang", string trangThai = null, string tuNgay = null, string denNgay = null)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var query = _context.TaiKhoans.Include(t => t.VaiTro).AsQueryable();

            if (!string.IsNullOrEmpty(role) && role != "All")
            {
                query = query.Where(t => t.VaiTro.TenVaiTro == role);
            }

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(t => t.TenDangNhap.Contains(searchStr));
            }

            if (!string.IsNullOrEmpty(trangThai) && trangThai != "All")
            {
                query = query.Where(t => t.TrangThai == trangThai);
            }

            if (!string.IsNullOrEmpty(tuNgay) && DateTime.TryParse(tuNgay, out DateTime dtTuNgay))
            {
                query = query.Where(t => t.NgayTao.Date >= dtTuNgay.Date);
            }
            if (!string.IsNullOrEmpty(denNgay) && DateTime.TryParse(denNgay, out DateTime dtDenNgay))
            {
                query = query.Where(t => t.NgayTao.Date <= dtDenNgay.Date);
            }

            var taiKhoans = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.SearchStr = searchStr;
            ViewBag.CurrentRole = role;
            ViewBag.CurrentTrangThai = trangThai;
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;

            ViewBag.CountKhachHang = await _context.TaiKhoans.CountAsync(t => t.VaiTro.TenVaiTro == "KhachHang");
            ViewBag.CountDoiTac = await _context.TaiKhoans.CountAsync(t => t.VaiTro.TenVaiTro == "DoiTac");
            ViewBag.CountAdmin = await _context.TaiKhoans.CountAsync(t => t.VaiTro.TenVaiTro == "Admin");

            return View(taiKhoans);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaiKhoanDetails(int id)
        {
            if (!CheckIsAdmin()) return Json(new { success = false });

            var tk = await _context.TaiKhoans
                .Include(t => t.VaiTro)
                .FirstOrDefaultAsync(t => t.MaTaiKhoan == id);

            if (tk == null) return Json(new { success = false });

            string roleName = tk.VaiTro?.TenVaiTro;
            object personalInfo = null;

            if (roleName == "KhachHang") 
            {
                var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTaiKhoan == id);
                if (kh != null) {
                    personalInfo = new {
                        ten = kh.TenKH,
                        sdt = kh.SoDTKH,
                        email = kh.EmailKH,
                        diaChi = kh.DiaChiCuThe,
                        thongTinThem = $"Điểm: {kh.DiemTichLuy}"
                    };
                }
            } 
            else if (roleName == "DoiTac") 
            {
                var dt = await _context.DoiTacs.FirstOrDefaultAsync(d => d.MaTaiKhoan == id);
                if (dt != null) {
                    personalInfo = new {
                        ten = dt.TenQuanDoiTac,
                        sdt = dt.SoDTDoiTac,
                        email = dt.EmailDTac,
                        diaChi = dt.DiaChiDoiTac,
                        thongTinThem = $"Trạng thái: {dt.TrangThaiDoiTac}"
                    };
                }
            }

            return Json(new {
                success = true,
                info = new {
                    username = tk.TenDangNhap,
                    role = roleName,
                    ngayTao = tk.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    trangThai = tk.TrangThai,
                    lyDoHuy = tk.LyDoHuy
                },
                personal = personalInfo
            });
        }

        // ===== QUẢN LÝ ĐIỀU LỆ =====
        [HttpGet]
        public async Task<IActionResult> QuanLyDieuLe(string searchStr, string sortOrder, string tuNgay, string denNgay)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            var query = _context.DieuLes.AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(d => d.TenDieuLe != null && d.TenDieuLe.Contains(searchStr));
            }

            if (!string.IsNullOrEmpty(tuNgay) && DateTime.TryParse(tuNgay, out DateTime dtTuNgay))
            {
                var fromDate = dtTuNgay.Date;
                query = query.Where(d => d.NgayThemDieuLe >= fromDate);
            }

            if (!string.IsNullOrEmpty(denNgay) && DateTime.TryParse(denNgay, out DateTime dtDenNgay))
            {
                var toDate = dtDenNgay.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.NgayThemDieuLe <= toDate);
            }

            if (sortOrder == "asc")
            {
                query = query.OrderBy(d => d.PhiChietKhau);
            }
            else if (sortOrder == "desc")
            {
                query = query.OrderByDescending(d => d.PhiChietKhau);
            }
            else
            {
                query = query.OrderByDescending(d => d.NgayThemDieuLe);
            }

            var dieuLes = await query.ToListAsync();

            ViewBag.SearchStr = searchStr;
            ViewBag.SortOrder = sortOrder;
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;

            return View(dieuLes);
        }

        [HttpPost]
        public async Task<IActionResult> ThemDieuLe([FromBody] DieuLe model)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });
            if (model == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });


            try
            {
                var dieuLe = new DieuLe
                {
                    TenDieuLe = model.TenDieuLe,
                    PhiChietKhau = model.PhiChietKhau,
                    ChiTietDieuLe = model.ChiTietDieuLe,
                    NgayThemDieuLe = DateTime.Now
                };

                _context.DieuLes.Add(dieuLe);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SuaDieuLe([FromBody] DieuLe model)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });
            if (model == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });


            try
            {
                var dieuLe = await _context.DieuLes.FindAsync(model.MaDieuLe);
                if (dieuLe == null)
                    return Json(new { success = false, message = "Không tìm thấy" });

                dieuLe.TenDieuLe = model.TenDieuLe;
                dieuLe.PhiChietKhau = model.PhiChietKhau;
                dieuLe.ChiTietDieuLe = model.ChiTietDieuLe;

                _context.Update(dieuLe);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> XoaDieuLe(int maDieuLe)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            try
            {
                var dieuLe = await _context.DieuLes.FindAsync(maDieuLe);
                if (dieuLe == null)
                    return Json(new { success = false, message = "Không tìm thấy" });

                _context.DieuLes.Remove(dieuLe);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ===== QUẢN LÝ MÓN ĂN =====
        [HttpGet]
        public async Task<IActionResult> QuanLyMonAn(int page = 1)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var monAns = await _context.MonAns
                .Include(m => m.GianHang)
                    .ThenInclude(g => g.DoiTac)
                .Include(m => m.TheLoaiMonAn)
                .Include(m => m.BienTheMonAns)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.MonAns.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(monAns);
        }


        [HttpGet]
        public async Task<IActionResult> GetBienTheMonAn(int maMonAn)
        {
            if (!CheckIsAdmin())
                return Unauthorized();

            var bienThes = await _context.BienTheMonAns
                .Where(b => b.MaMonAn == maMonAn)
                .ToListAsync();

            return Json(bienThes);
        }

        // ===== QUẢN LÝ ĐƠN HÀNG =====
        [HttpGet]
        public async Task<IActionResult> QuanLyDonHang(int page = 1)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            const int pageSize = 10;
            var donHangs = await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.GianHang)
                .OrderByDescending(d => d.NgayTaoDon)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.DonHangs.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(donHangs);
        }

        [HttpGet]
        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.GianHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(bt => bt.MonAn)
                .FirstOrDefaultAsync(d => d.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        // ===== API DOANH THU THÁNG =====
        [HttpGet]
        public async Task<IActionResult> GetDoanhThu(int maGianHang, int? thang, int? nam)
        {
            if (!CheckIsAdmin())
                return Unauthorized();

            var now = DateTime.Now;
            int selMonth = thang ?? now.Month;
            int selYear = nam ?? now.Year;

            var firstDayOfMonth = new DateTime(selYear, selMonth, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var donHangs = await _context.DonHangs
                .Where(d => d.MaGianHang == maGianHang
                    && d.NgayTaoDon >= firstDayOfMonth
                    && d.NgayTaoDon <= lastDayOfMonth.AddHours(23).AddMinutes(59).AddSeconds(59)
                    && d.TrangThaiDonHang != "Đã hủy" && d.TrangThaiDonHang != "Từ chối")
                .ToListAsync();

            var tongDoanhThu = donHangs.Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao").Sum(d => d.ThanhTienKhachTra);
            var tongDonHang = donHangs.Count;
            var donThanhCong = donHangs.Count(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao");
            var donChoXuLy = donHangs.Count(d => d.TrangThaiDonHang == "Chờ xác nhận" || d.TrangThaiDonHang == "Đã xác nhận" || d.TrangThaiDonHang == "Đang chuẩn bị" || d.TrangThaiDonHang == "Đang giao");

            // Doanh thu theo ngày trong tháng đó
            var daysInMonth = DateTime.DaysInMonth(selYear, selMonth);
            var dailyRevenue = new List<object>();
            
            var revenueByDay = donHangs.Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao")
                                     .GroupBy(d => d.NgayTaoDon.Day)
                                     .Select(g => new { Day = g.Key, Amount = g.Sum(x => x.ThanhTienKhachTra) })
                                     .ToList();

            for (int i = 1; i <= daysInMonth; i++)
            {
                var r = revenueByDay.FirstOrDefault(x => x.Day == i);
                dailyRevenue.Add(new {
                    ngay = $"{i:D2}/{selMonth:D2}",
                    doanhThu = r != null ? r.Amount : 0
                });
            }

            return Json(new
            {
                tongDoanhThu,
                tongDonHang,
                donThanhCong,
                donChoXuLy,
                thangHienTai = $"{selMonth:D2}/{selYear}",
                dailyRevenue
            });
        }

        // ===== QUẢN LÝ KHUYẾN MÃI =====
        [HttpGet]
        public async Task<IActionResult> QuanLyKhuyenMai(string searchStr, string sortOrder, string tuNgay, string denNgay, int page = 1)
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");

            var query = _context.ChuongTrinhKhuyenMais.AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                query = query.Where(k => k.TenChTrinh != null && k.TenChTrinh.Contains(searchStr));
            }

            if (!string.IsNullOrEmpty(tuNgay) && DateTime.TryParse(tuNgay, out DateTime dtTuNgay))
            {
                var fromDate = dtTuNgay.Date;
                query = query.Where(k => k.NgayBatDau >= fromDate);
            }

            if (!string.IsNullOrEmpty(denNgay) && DateTime.TryParse(denNgay, out DateTime dtDenNgay))
            {
                var toDate = dtDenNgay.Date.AddDays(1).AddTicks(-1);
                query = query.Where(k => k.NgayBatDau <= toDate);
            }

            if (sortOrder == "asc")
            {
                query = query.OrderBy(k => k.PhanTramGiam);
            }
            else if (sortOrder == "desc")
            {
                query = query.OrderByDescending(k => k.PhanTramGiam);
            }
            else
            {
                query = query.OrderByDescending(k => k.MaKMai);
            }

            const int pageSize = 10;
            var totalCount = await query.CountAsync();
            var ChuongTrinhKhuyenMais = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.SearchStr = searchStr;
            ViewBag.SortOrder = sortOrder;
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(ChuongTrinhKhuyenMais);
        }

        [HttpPost]
        public async Task<IActionResult> ThemKhuyenMai([FromBody] ChuongTrinhKhuyenMai model)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            if (model == null || string.IsNullOrWhiteSpace(model.NoiDungKMai) || string.IsNullOrWhiteSpace(model.TenChTrinh))
                return Json(new { success = false, message = "Dữ liệu không hợp lệ hoặc mã rỗng" });

            try
            {
                var noiDung = model.NoiDungKMai.ToUpper();
                // Kiểm tra mã giảm giá trùng
                var existing = await _context.ChuongTrinhKhuyenMais
                    .FirstOrDefaultAsync(k => k.NoiDungKMai == noiDung);
                if (existing != null)
                    return Json(new { success = false, message = "Mã giảm giá đã tồn tại!" });

                var khuyenMai = new ChuongTrinhKhuyenMai
                {
                    TenChTrinh = model.TenChTrinh,
                    NoiDungKMai = noiDung,
                    PhanTramGiam = model.PhanTramGiam,
                    GiamToiDa = model.GiamToiDa,
                    NgayBatDau = model.NgayBatDau, NgayKetThuc = model.NgayKetThuc,
                    TrangThaiKMai = "Mới nhất"
                };

                _context.ChuongTrinhKhuyenMais.Add(khuyenMai);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SuaKhuyenMai([FromBody] ChuongTrinhKhuyenMai model)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });
            if (model == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (model.NgayKetThuc <= model.NgayBatDau)
                return Json(new { success = false, message = "Ngày kết thúc phải sau ngày bắt đầu." });
            if (model.PhanTramGiam <= 0 || model.PhanTramGiam > 100)
                return Json(new { success = false, message = "Phần trăm giảm phải lớn hơn 0 và nhỏ hơn hoặc bằng 100." });


            try
            {
                var khuyenMai = await _context.ChuongTrinhKhuyenMais.FindAsync(model.MaKMai);
                if (khuyenMai == null)
                    return Json(new { success = false, message = "Không tìm thấy khuyến mãi" });

                khuyenMai.TenChTrinh = model.TenChTrinh;
                khuyenMai.NoiDungKMai = model.NoiDungKMai.ToUpper();
                khuyenMai.PhanTramGiam = model.PhanTramGiam;
                khuyenMai.GiamToiDa = model.GiamToiDa;
                khuyenMai.NgayBatDau = model.NgayBatDau; khuyenMai.NgayKetThuc = model.NgayKetThuc;
                khuyenMai.TrangThaiKMai = model.TrangThaiKMai;

                _context.Update(khuyenMai);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> XoaKhuyenMai(int MaKMai)
        {
            if (!CheckIsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            try
            {
                var khuyenMai = await _context.ChuongTrinhKhuyenMais.FindAsync(MaKMai);
                if (khuyenMai == null)
                    return Json(new { success = false, message = "Không tìm thấy" });

                _context.ChuongTrinhKhuyenMais.Remove(khuyenMai);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    
        public IActionResult SaoKeDoanhThu()
        {
            if (!CheckIsAdmin())
                return RedirectToAction("DangNhap", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDoanhThuData(string tuNgay, string denNgay, decimal? minAmount, decimal? maxAmount, string sortOrder)
        {
            if (!CheckIsAdmin()) return Unauthorized();

            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var query = _context.DonHangs
                .Include(d => d.GianHang)
                .Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao");

            if (!string.IsNullOrEmpty(tuNgay) && DateTime.TryParse(tuNgay, out DateTime dtTuNgay))
            {
                query = query.Where(d => d.NgayTaoDon.Date >= dtTuNgay.Date);
            }
            if (!string.IsNullOrEmpty(denNgay) && DateTime.TryParse(denNgay, out DateTime dtDenNgay))
            {
                query = query.Where(d => d.NgayTaoDon.Date <= dtDenNgay.Date);
            }
            if (minAmount.HasValue)
            {
                query = query.Where(d => d.ThanhTienKhachTra >= minAmount.Value);
            }
            if (maxAmount.HasValue)
            {
                query = query.Where(d => d.ThanhTienKhachTra <= maxAmount.Value);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(m => m.MaDonHang.ToString().Contains(searchValue));
            }

            int recordsTotal = await query.CountAsync();

            if (sortOrder == "asc")
            {
                query = query.OrderBy(d => d.ThanhTienKhachTra);
            }
            else if (sortOrder == "desc")
            {
                query = query.OrderByDescending(d => d.ThanhTienKhachTra);
            }
            else
            {
                query = query.OrderByDescending(d => d.NgayTaoDon);
            }

            var dataList = await query.Skip(skip).Take(pageSize).ToListAsync();

            var data = dataList.Select(d => new
            {
                maDonHang = d.MaDonHang,
                tenQuan = d.GianHang != null ? d.GianHang.TenGianHang : "N/A",
                ngayTao = d.NgayTaoDon.ToString("dd/MM/yyyy HH:mm"),
                tongTienMon = d.TongTienMon,
                giamGia = d.GiamGia,
                phiDichVuSan = d.PhiDichVuSan,
                phiShip = d.PhiShip,
                thanhTienKhachTra = d.ThanhTienKhachTra,
                thanhTienQuanNhan = d.ThanhTienQuanNhan
            }).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThaiTaiKhoan(int maTaiKhoan, string lyDo)
        {
            if (!CheckIsAdmin()) return Json(new { success = false, message = "Unauthorized" });

            var taiKhoan = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            if (taiKhoan == null) return Json(new { success = false, message = "Không tìm thấy tài khoản" });

            if (taiKhoan.MaVaiTro == _context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Admin")?.MaVaiTro)
            {
                return Json(new { success = false, message = "Không thể khóa tài khoản Admin" });
            }

            taiKhoan.TrangThai = "Bị khóa";
            taiKhoan.LyDoHuy = lyDo;
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã khóa tài khoản thành công" });
        }

        [HttpPost]
        public async Task<IActionResult> MoKhoaTaiKhoan(int maTaiKhoan)
        {
            if (!CheckIsAdmin()) return Json(new { success = false, message = "Unauthorized" });

            var taiKhoan = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            if (taiKhoan == null) return Json(new { success = false, message = "Không tìm thấy tài khoản" });

            taiKhoan.TrangThai = "Hoạt động";
            taiKhoan.LyDoHuy = null;
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã mở khóa tài khoản thành công" });
        }
    }
}
