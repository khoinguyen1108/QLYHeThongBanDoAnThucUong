using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models.Entities;
using QuanLyBanHangDoAnThucUong.Models.ViewModels;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public class QuanLyGianHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuanLyGianHangController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> ProcessImage(IFormFile file, string folder)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string finalFileName = fileName + extension;
            string path = Path.Combine(wwwRootPath, "images", folder, finalFileName);

            int count = 1;
            while (System.IO.File.Exists(path))
            {
                finalFileName = $"{fileName}_{count:D2}{extension}";
                path = Path.Combine(wwwRootPath, "images", folder, finalFileName);
                count++;
            }

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return finalFileName;
        }

        // ===== KIỂM TRA QUYỀN =====
        private bool CheckIsDoiTac()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            return maTaiKhoan.HasValue && vaiTro == "DoiTac";
        }

        private async Task<DoiTac> GetCurrentDoiTac()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (!maTaiKhoan.HasValue)
                return null;

            return await _context.DoiTacs
                .FirstOrDefaultAsync(d => d.MaTaiKhoan == maTaiKhoan.Value);
        }

        // ===== TRANG CHỦ QUẢN LÝ =====
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null)
                return NotFound();

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();

            if (gianHang == null)
                return NotFound();

            // --- THỐNG KÊ ---
            var orders = await _context.DonHangs
                .Where(d => d.MaGianHang == gianHang.MaGianHang)
                .ToListAsync();

            ViewBag.PendingOrdersCount = orders.Count(d => d.TrangThaiDonHang == "Chờ xác nhận" || d.TrangThaiDonHang == "Đang chuẩn bị");
            
            var today = DateTime.Today;
            ViewBag.DailyRevenue = orders
                .Where(d => d.NgayTaoDon.Date == today && d.TrangThaiDonHang == "Hoàn thành")
                .Sum(d => d.ThanhTienKhachTra);

            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            ViewBag.MonthlyRevenue = orders
                .Where(d => d.NgayTaoDon >= startOfMonth && d.TrangThaiDonHang == "Hoàn thành")
                .Sum(d => d.ThanhTienKhachTra);

            var startOfYear = new DateTime(today.Year, 1, 1);
            ViewBag.YearlyRevenue = orders
                .Where(d => d.NgayTaoDon >= startOfYear && d.TrangThaiDonHang == "Hoàn thành")
                .Sum(d => d.ThanhTienKhachTra);
                
            ViewBag.TotalRevenue = orders
                .Where(d => d.TrangThaiDonHang == "Hoàn thành")
                .Sum(d => d.ThanhTienKhachTra);

            var phuongXas = await _context.DiaChis
                .Where(d => !string.IsNullOrEmpty(d.PhuongXa))
                .Select(d => d.PhuongXa)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();
            ViewBag.PhuongXas = phuongXas;

            return View(gianHang);
        }

        // ===== THỐNG KÊ DOANH THU =====
        [HttpGet]
        public async Task<IActionResult> DoanhThu(DateTime? fromDate, DateTime? toDate, string status, string sortOrder)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null)
                return NotFound();

            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac);
            if (gianHang == null)
                return NotFound();

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.Status = status;
            ViewBag.SortOrder = sortOrder;

            var ordersQuery = _context.DonHangs.Include(d => d.KhachHang).Where(d => d.MaGianHang == gianHang.MaGianHang);

            if (fromDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(d => d.NgayTaoDon.Date >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(d => d.NgayTaoDon.Date <= toDate.Value.Date);
            }
            if (!string.IsNullOrEmpty(status))
            {
                ordersQuery = ordersQuery.Where(d => d.TrangThaiDonHang == status);
            }

            var orders = await ordersQuery.ToListAsync();
            if (sortOrder == "asc")
                orders = orders.OrderBy(d => d.ThanhTienKhachTra).ToList();
            else if (sortOrder == "desc")
                orders = orders.OrderByDescending(d => d.ThanhTienKhachTra).ToList();

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);
            var firstDayOfYear = new DateTime(today.Year, 1, 1);
            var firstDayOfLastYear = firstDayOfYear.AddYears(-1);

            ViewBag.TotalOrdersCount = orders.Count;
            ViewBag.PendingOrdersCount = orders.Count(d => d.TrangThaiDonHang == "Chờ xác nhận" || d.TrangThaiDonHang == "Đang chuẩn bị");
            ViewBag.DeliveryOrdersCount = orders.Count(d => d.TrangThaiDonHang == "Đang giao");
            ViewBag.CanceledOrdersCount = orders.Count(d => d.TrangThaiDonHang == "Đã hủy" || d.TrangThaiDonHang == "Từ chối");

            var completedOrders = orders.Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao").ToList();

            ViewBag.DailyRevenue = completedOrders.Where(d => d.NgayTaoDon.Date == today).Sum(d => d.ThanhTienKhachTra);
            ViewBag.YesterdayRevenue = completedOrders.Where(d => d.NgayTaoDon.Date == yesterday).Sum(d => d.ThanhTienKhachTra);
            
            ViewBag.MonthlyRevenue = completedOrders.Where(d => d.NgayTaoDon >= firstDayOfMonth).Sum(d => d.ThanhTienKhachTra);
            ViewBag.LastMonthRevenue = completedOrders.Where(d => d.NgayTaoDon >= firstDayOfLastMonth && d.NgayTaoDon < firstDayOfMonth).Sum(d => d.ThanhTienKhachTra);
            
            ViewBag.YearlyRevenue = completedOrders.Where(d => d.NgayTaoDon >= firstDayOfYear).Sum(d => d.ThanhTienKhachTra);
            ViewBag.LastYearRevenue = completedOrders.Where(d => d.NgayTaoDon >= firstDayOfLastYear && d.NgayTaoDon < firstDayOfYear).Sum(d => d.ThanhTienKhachTra);
            
            ViewBag.TotalRevenue = completedOrders.Sum(d => d.ThanhTienKhachTra);

            ViewBag.RecentOrdersCount = orders.Count;
            ViewBag.RecentOrders = orders.OrderByDescending(d => d.NgayTaoDon).Take(20).ToList();

            return View(gianHang);
        }

        // ===== QUẢN LÝ KHUYẾN MÃI =====
        [HttpGet]
        public async Task<IActionResult> ChuongTrinhKhuyenMai(string searchStr, string sortOrder, string tuNgay, string denNgay)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return NotFound();

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();
            if (gianHang == null) return NotFound();

            var query = _context.ChuongTrinhKhuyenMais
                .Where(m => m.MaGianHang == gianHang.MaGianHang)
                .AsQueryable();

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
                query = query.OrderByDescending(k => k.NgayBatDau);
            }

            var vouchers = await query.ToListAsync();

            ViewBag.Vouchers = vouchers;
            ViewBag.SearchStr = searchStr;
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;
            ViewBag.SortOrder = sortOrder;

            return View(gianHang);
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhuyenMai([FromBody] ChuongTrinhKhuyenMai model)
        {
            if (!CheckIsDoiTac())
                return Json(new { success = false, message = "Unauthorized" });
            if (model == null || string.IsNullOrWhiteSpace(model.NoiDungKMai))
                return Json(new { success = false, message = "Dữ liệu không hợp lệ hoặc mã rỗng" });

            if (model.NgayKetThuc <= model.NgayBatDau)
                return Json(new { success = false, message = "Ngày kết thúc phải sau ngày bắt đầu." });
            if (model.PhanTramGiam <= 0 || model.PhanTramGiam > 100)
                return Json(new { success = false, message = "Phần trăm giảm phải lớn hơn 0 và nhỏ hơn hoặc bằng 100." });


            try
            {
                var noiDung = model.NoiDungKMai.ToUpper();
                var existing = await _context.ChuongTrinhKhuyenMais.FirstOrDefaultAsync(k => k.NoiDungKMai == noiDung);
                if (existing != null)
                    return Json(new { success = false, message = "Mã giảm giá đã tồn tại!" });

                var doiTac = await GetCurrentDoiTac();
                var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac).FirstOrDefaultAsync();
                if (gianHang == null) return Json(new { success = false, message = "Store not found" });

                model.NoiDungKMai = noiDung;
                model.MaGianHang = gianHang.MaGianHang;
                
                _context.ChuongTrinhKhuyenMais.Add(model);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Lỗi hệ thống: " + inner });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKhuyenMai([FromBody] ChuongTrinhKhuyenMai model)
        {
            if (!CheckIsDoiTac())
                return Json(new { success = false, message = "Unauthorized" });
            if (model == null)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (model.NgayKetThuc <= model.NgayBatDau)
                return Json(new { success = false, message = "Ngày kết thúc phải sau ngày bắt đầu." });
            if (model.PhanTramGiam <= 0 || model.PhanTramGiam > 100)
                return Json(new { success = false, message = "Phần trăm giảm phải lớn hơn 0 và nhỏ hơn hoặc bằng 100." });


            var km = await _context.ChuongTrinhKhuyenMais.FirstOrDefaultAsync(k => k.MaKMai == model.MaKMai);
            if (km == null) return Json(new { success = false, message = "Not found" });

            km.TenChTrinh = model.TenChTrinh;
            km.NoiDungKMai = model.NoiDungKMai;
            km.PhanTramGiam = model.PhanTramGiam;
            km.GiamToiDa = model.GiamToiDa;
            km.NgayBatDau = model.NgayBatDau;
            km.NgayKetThuc = model.NgayKetThuc;
            km.TrangThaiKMai = model.TrangThaiKMai;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteKhuyenMai(int id)
        {
            if (!CheckIsDoiTac()) return Json(new { success = false, message = "Unauthorized" });

            var km = await _context.ChuongTrinhKhuyenMais.FirstOrDefaultAsync(k => k.MaKMai == id);
            if (km == null) return Json(new { success = false, message = "Not found" });

            _context.ChuongTrinhKhuyenMais.Remove(km);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenueData(string period = "7days", string start = null, string end = null)
        {
            if (!CheckIsDoiTac())
                return Json(new { success = false, message = "Unauthorized" });

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return Json(new { success = false, message = "Partner not found" });

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();
            if (gianHang == null) return Json(new { success = false, message = "Store not found" });

            var completedOrders = await _context.DonHangs
                .Where(d => d.MaGianHang == gianHang.MaGianHang && d.TrangThaiDonHang == "Hoàn thành" && d.TrangThaiThanhToan == "Đã thanh toán")
                .ToListAsync();

            var query = _context.DonHangs
                .Where(d => d.MaGianHang == gianHang.MaGianHang && d.TrangThaiDonHang == "Hoàn thành" && d.TrangThaiThanhToan == "Đã thanh toán");

            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;

            if (period == "30days")
            {
                startDate = DateTime.Today.AddDays(-29);
            }
            else if (period == "month")
            {
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            else if (period == "prevmonth")
            {
                var prev = DateTime.Today.AddMonths(-1);
                startDate = new DateTime(prev.Year, prev.Month, 1);
                endDate = new DateTime(prev.Year, prev.Month, DateTime.DaysInMonth(prev.Year, prev.Month));
            }
            else if (period == "custom" && !string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                if (DateTime.TryParse(start, out var sDate) && DateTime.TryParse(end, out var eDate))
                {
                    startDate = sDate;
                    endDate = eDate;
                }
            }

            var orders = await query
                .Where(d => d.NgayTaoDon >= startDate && d.NgayTaoDon <= endDate.AddDays(1).AddTicks(-1))
                .ToListAsync();

            var revenues = new List<decimal>();
            var labels = new List<string>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var revenue = orders
                    .Where(d => d.NgayTaoDon.Date == date)
                    .Sum(d => d.ThanhTienKhachTra);
                revenues.Add(revenue);
                labels.Add(date.ToString("dd/MM"));
            }

            return Json(new { success = true, labels, revenues, total = revenues.Sum() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatThongTinDoiTac(string tenQuan, string diaChi, string soDT, string thanhPho, string phuongXa)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return NotFound();

            doiTac.TenQuanDoiTac = tenQuan;
            doiTac.DiaChiDoiTac = diaChi;
            doiTac.SoDTDoiTac = soDT;

            _context.Update(doiTac);
            
            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac);
            if (gianHang != null)
            {
                gianHang.ThanhPho = thanhPho;
                gianHang.PhuongXa = phuongXa;
                _context.Update(gianHang);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Index");
        }

        // ===== QUẢN LÝ MÓN ĂN =====
        [HttpGet]
        public async Task<IActionResult> QuanLyMonAn(string status)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null)
                return NotFound();

            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac);
            if (gianHang == null)
                return NotFound();

            ViewBag.Status = status;

            var monAnQuery = _context.MonAns
                .Include(m => m.TheLoaiMonAn)
                .Include(m => m.BienTheMonAns)
                .Where(m => m.MaGianHang == gianHang.MaGianHang);

            var allMonAns = await monAnQuery.ToListAsync();

            ViewBag.TotalTotal = allMonAns.Count;
            ViewBag.TotalTheLoai = allMonAns.Select(m => m.MaLoaiMon).Distinct().Count();
            ViewBag.TotalDangBan = allMonAns.SelectMany(m => m.BienTheMonAns).Count(b => b.TrangThaiMonAn == "Có sẵn" || b.TrangThaiMonAn == "Còn bán");
            ViewBag.TotalHetMon = allMonAns.SelectMany(m => m.BienTheMonAns).Count(b => b.TrangThaiMonAn != "Có sẵn" && b.TrangThaiMonAn != "Còn bán");

            if (!string.IsNullOrEmpty(status))
            {
                if(status == "Có sẵn")
                {
                    allMonAns = allMonAns.Where(m => m.BienTheMonAns.Any(b => b.TrangThaiMonAn == "Có sẵn" || b.TrangThaiMonAn == "Còn bán")).ToList();
                }
                else if(status == "Hết món")
                {
                    allMonAns = allMonAns.Where(m => m.BienTheMonAns.All(b => b.TrangThaiMonAn != "Có sẵn" && b.TrangThaiMonAn != "Còn bán")).ToList();
                }
            }

            ViewBag.MonAns = allMonAns;

            return View(gianHang);
        }

        // ===== THÊM MÓN ĂN =====
        [HttpGet]
        public async Task<IActionResult> ThemMonAn()
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var loaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
            ViewBag.LoaiMonAn = loaiMonAn;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemMonAn(ThemMonAnViewModel model)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
                return View(model);
            }

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null)
                return NotFound();

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();

            if (gianHang == null)
                return NotFound();

            // Xử lý hình ảnh
            string hinhAnh = "default-food.png";
            if (model.FileHinh != null)
            {
                hinhAnh = await ProcessImage(model.FileHinh, "monan");
            }

            // Tạo món ăn
            var monAn = new MonAn
            {
                TenMon = model.TenMon,
                MaGianHang = gianHang.MaGianHang,
                MaLoaiMon = model.MaLoaiMon
            };

            _context.MonAns.Add(monAn);
            await _context.SaveChangesAsync();

            // Tạo biến thể
            var bienThe = new BienTheMonAn
            {
                MaMonAn = monAn.MaMonAn,
                MoTaMonAn = model.MoTaMonAn,
                GiaBan = model.GiaBan,
                SoLuongMon = model.SoLuongMon,
                HinhAnhMonAn = hinhAnh,
                TrangThaiMonAn = "Còn bán",
                GhiChu = model.GhiChu,
                MonAnMuaThem = model.MonAnMuaThem
            };

            _context.BienTheMonAns.Add(bienThe);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm món ăn thành công!";
            return RedirectToAction("QuanLyMonAn");
        }

        // ===== CHỈNH SỬA MÓN ĂN =====
        // ===== CHỈNH SỬA MÓN ĂN =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaMonAn(SuaMonAnViewModel model)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
                return View(model);
            }

            try
            {
                // Lấy món ăn
                var monAn = await _context.MonAns.FindAsync(model.MaMonAn);
                if (monAn == null)
                    return NotFound();

                // Lấy biến thể
                var bienThe = await _context.BienTheMonAns.FindAsync(model.MaBienThe);
                if (bienThe == null)
                    return NotFound();

                // Cập nhật thông tin món ăn
                monAn.TenMon = model.TenMon;
                monAn.MaLoaiMon = model.MaLoaiMon;

                // Xử lý hình ảnh mới nếu có
                if (model.FileHinh != null)
                {
                    bienThe.HinhAnhMonAn = await ProcessImage(model.FileHinh, "monan");
                }

                // Cập nhật thông tin biến thể
                bienThe.MoTaMonAn = model.MoTaMonAn;
                bienThe.GiaBan = model.GiaBan;
                bienThe.SoLuongMon = model.SoLuongMon;
                bienThe.GhiChu = model.GhiChu;
                bienThe.MonAnMuaThem = model.MonAnMuaThem;
                bienThe.TrangThaiMonAn = model.TrangThaiMonAn ?? "Còn bán";

                _context.Update(monAn);
                _context.Update(bienThe);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật món ăn thành công!";
                return RedirectToAction("QuanLyMonAn");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
                ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
                return View(model);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SuaMonAn(int id, SuaMonAnViewModel model)
        //{
        //    if (!CheckIsDoiTac())
        //        return RedirectToAction("DangNhap", "Account");

        //    if (id != model.MaMonAn)
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
        //        return View(model);
        //    }

        //    try
        //    {
        //        // Lấy món ăn
        //        var monAn = await _context.MonAns.FindAsync(id);
        //        if (monAn == null)
        //            return NotFound();

        //        // Lấy biến thể
        //        var bienThe = await _context.BienTheMonAns.FindAsync(model.MaBienThe);
        //        if (bienThe == null)
        //            return NotFound();

        //        // Cập nhật thông tin món ăn
        //        monAn.TenMon = model.TenMon;
        //        monAn.MaLoaiMon = model.MaLoaiMon;

        //        // Cập nhật thông tin biến thể
        //        bienThe.MoTaMonAn = model.MoTaMonAn;
        //        bienThe.GiaBan = model.GiaBan;
        //        bienThe.SoLuongMon = model.SoLuongMon;
        //        bienThe.HinhAnhMonAn = model.HinhAnhMonAn ?? "default.jpg";
        //        bienThe.GhiChu = model.GhiChu;
        //        bienThe.MonAnMuaThem = model.MonAnMuaThem;
        //        bienThe.TrangThaiMonAn = model.TrangThaiMonAn;

        //        _context.Update(monAn);
        //        _context.Update(bienThe);
        //        await _context.SaveChangesAsync();

        //        TempData["Success"] = "Cập nhật món ăn thành công!";
        //        return RedirectToAction("QuanLyMonAn");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
        //        ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();
        //        return View(model);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> SuaMonAn(int id)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var monAn = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .FirstOrDefaultAsync(m => m.MaMonAn == id);

            if (monAn == null)
                return NotFound();

            var bienThe = monAn.BienTheMonAns.FirstOrDefault();

            var model = new SuaMonAnViewModel
            {
                MaMonAn = monAn.MaMonAn,
                TenMon = monAn.TenMon,
                MaLoaiMon = monAn.MaLoaiMon,

                MaBienThe = bienThe?.MaBienThe ?? 0,
                MoTaMonAn = bienThe?.MoTaMonAn,
                GiaBan = bienThe?.GiaBan ?? 0,
                SoLuongMon = bienThe?.SoLuongMon ?? 0,
                HinhAnhMonAn = bienThe?.HinhAnhMonAn,
                GhiChu = bienThe?.GhiChu,
                MonAnMuaThem = bienThe?.MonAnMuaThem,
                TrangThaiMonAn = bienThe?.TrangThaiMonAn
            };

            ViewBag.LoaiMonAn = await _context.TheLoaiMonAns.ToListAsync();

            return View(model);
        }

        // ===== XÓA MÓN ĂN =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaMonAn(int id)
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var monAn = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .FirstOrDefaultAsync(m => m.MaMonAn == id);

            if (monAn == null)
                return NotFound();

            try
            {
                _context.MonAns.Remove(monAn);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Xóa món ăn thành công!";
                return RedirectToAction("QuanLyMonAn");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
                return RedirectToAction("QuanLyMonAn");
            }
        }
        // ===== QUẢN LÝ ĐƠN HÀNG =====
        [HttpGet]
        public async Task<IActionResult> QuanLyDonHang()
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return NotFound();

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();

            if (gianHang == null) return NotFound();

            var donHangs = await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(b => b.MonAn)
                .Where(d => d.MaGianHang == gianHang.MaGianHang)
                .OrderByDescending(d => d.NgayTaoDon)
                .ToListAsync();

            return View(donHangs);
        }

        [HttpGet]
        public async Task<IActionResult> QuanLyDanhGia()
        {
            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return RedirectToAction("DangNhap", "Account");
            var maDT = doiTac.MaDoiTac;
            if (maDT == null) return RedirectToAction("Login", "Account");

            var gianHang = await _context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == maDT);
            if (gianHang == null) return NotFound();

            var danhGias = await _context.DanhGiaMonAns
                .Include(d => d.MonAn)
                .Include(d => d.KhachHang)
                .Where(d => d.MonAn.MaGianHang == gianHang.MaGianHang)
                .OrderByDescending(d => d.NgayDanhGia)
                .ToListAsync();

            return View(danhGias);
        }

        [HttpPost]
        public async Task<IActionResult> PhanHoiDanhGia(int maDanhGia, string noiDung)
        {
            var danhGia = await _context.DanhGiaMonAns.FindAsync(maDanhGia);
            if (danhGia == null) return Json(new { success = false, message = "Không tìm thấy đánh giá" });

            danhGia.PhanHoiCuaDoiTac = noiDung;
            danhGia.NgayPhanHoi = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, ngayPhanHoi = danhGia.NgayPhanHoi.Value.ToString("dd/MM/yyyy HH:mm") });
        }

        [HttpPost]
        public async Task<IActionResult> AnDanhGia(int maDanhGia)
        {
            var danhGia = await _context.DanhGiaMonAns.FindAsync(maDanhGia);
            if (danhGia == null) return Json(new { success = false, message = "Không tìm thấy đánh giá" });

            danhGia.TrangThaiHienThi = false;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        public async Task<IActionResult> CapNhatTrangThai(int id, string loai, string giaTri, string? lyDo = null)
        {
            if (!CheckIsDoiTac()) return Json(new { success = false, message = "Không có quyền." });

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null) return Json(new { success = false, message = "Đơn hàng không tồn tại." });

            if (loai == "DonHang")
            {
                                if (giaTri == "Đã hủy" && string.IsNullOrWhiteSpace(lyDo))
                    return Json(new { success = false, message = "Vui lòng nhập lý do hủy đơn hàng." });

                donHang.TrangThaiDonHang = giaTri;
                
                if (giaTri == "Đã hủy")
                {
                    donHang.LyDoHuy = lyDo;
                }
                if (giaTri == "Hoàn thành")
                {
                    donHang.TrangThaiThanhToan = "Đã thanh toán";
                }
            }
            else if (loai == "ThanhToan")
            {
                donHang.TrangThaiThanhToan = giaTri;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingOrdersCount()
        {
            if (!CheckIsDoiTac()) return Json(new { success = false, count = 0 });

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return Json(new { success = false, count = 0 });

            var gianHang = await _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .Include(g => g.MonAns)
                .FirstOrDefaultAsync();
            if (gianHang == null) return Json(new { success = false, count = 0 });

            var count = await _context.DonHangs.CountAsync(d => d.MaGianHang == gianHang.MaGianHang && d.TrangThaiDonHang == "Chờ xác nhận");
            return Json(new { success = true, count = count });
        }
    
        public async Task<IActionResult> DanhSachChiNhanh()
        {
            if (!CheckIsDoiTac())
                return RedirectToAction("DangNhap", "Account");

            var doiTac = await GetCurrentDoiTac();
            if (doiTac == null) return NotFound();

            var dsChiNhanh = await _context.GianHangs
                .Where(g => g.MaDoiTac == doiTac.MaDoiTac)
                .ToListAsync();

            return View(dsChiNhanh);
        }
    }
}




