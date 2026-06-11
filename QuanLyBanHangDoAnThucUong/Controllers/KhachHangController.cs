using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models.Entities;
using QuanLyBanHangDoAnThucUong.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    [Route("khachhang")]
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhachHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== KIỂM TRA QUYỀN =====
        private bool CheckIsKhachHang()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            return maTaiKhoan.HasValue && vaiTro == "KhachHang";
        }

        private async Task<KhachHang> GetCurrentKhachHang()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (!maTaiKhoan.HasValue)
                return null;

            return await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan.Value);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null)
                return RedirectToAction("BuyerDangNhap", "Account");

            return View(khachHang);
        }

        // ===== THÔNG TIN KHÁCH HÀNG =====
        [HttpGet("chinh-sua-ho-so")]
        public async Task<IActionResult> ThongTinKhachHang()
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null) return RedirectToAction("BuyerDangNhap", "Account");

            var model = new ThongTinKhachHangViewModel
            {
                TenKH = khachHang.TenKH,
                Email = khachHang.EmailKH,
                SoDienThoai = khachHang.SoDTKH,
                DiaChi = khachHang.DiaChiCuThe,
                GioiTinh = khachHang.GioiTinhKH,
                ThanhPho = khachHang.ThanhPho,
                PhuongXa = khachHang.PhuongXa
            };

            var phuongXas = await _context.DiaChis
                .Where(d => !string.IsNullOrEmpty(d.PhuongXa))
                .Select(d => d.PhuongXa)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();
            ViewBag.PhuongXas = phuongXas;

            return View(model);
        }

        [HttpPost("chinh-sua-ho-so")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThongTinKhachHang(ThongTinKhachHangViewModel model)
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            if (!ModelState.IsValid)
            {
                var phuongXas = await _context.DiaChis
                    .Where(d => !string.IsNullOrEmpty(d.PhuongXa))
                    .Select(d => d.PhuongXa)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToListAsync();
                ViewBag.PhuongXas = phuongXas;
                return View(model);
            }

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null) return RedirectToAction("BuyerDangNhap", "Account");

            var taiKhoan = await _context.TaiKhoans.FindAsync(khachHang.MaTaiKhoan);

            // Cập nhật thông tin cá nhân
            khachHang.TenKH = model.TenKH;
            khachHang.EmailKH = model.Email;
            khachHang.SoDTKH = model.SoDienThoai;
            khachHang.DiaChiCuThe = model.DiaChi;
            khachHang.GioiTinhKH = model.GioiTinh;
            khachHang.ThanhPho = model.ThanhPho;
            khachHang.PhuongXa = model.PhuongXa;

            // Kiểm tra đổi mật khẩu
            if (!string.IsNullOrEmpty(model.MatKhauCu) && !string.IsNullOrEmpty(model.MatKhauMoi))
            {
                // Verify mật khẩu cũ (tương tự AccountController)
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.MatKhauCu));
                    var hashOfInput = Convert.ToBase64String(hashedBytes);

                    if (taiKhoan.MatKhau != model.MatKhauCu && taiKhoan.MatKhau != hashOfInput)
                    {
                        ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không chính xác");
                        return View(model);
                    }

                    // Đổi mật khẩu mới
                    var newHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.MatKhauMoi));
                    taiKhoan.MatKhau = Convert.ToBase64String(newHashedBytes);
                    _context.Update(taiKhoan);
                }
            }

            _context.Update(khachHang);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin cá nhân thành công!";
            return RedirectToAction("ThongTinKhachHang");
        }

        // ===== QUẢN LÝ ĐƠN MUA =====
        [HttpGet("don-mua")]
        public async Task<IActionResult> DonMua(string status = "all", int page = 1)
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null) return RedirectToAction("BuyerDangNhap", "Account");

            var query = _context.DonHangs
                .Include(d => d.GianHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BienTheMonAn)
                        .ThenInclude(bt => bt.MonAn)
                .Where(d => d.MaKH == khachHang.MaKH);

            // Filter by status if not 'all'
            if (status != "all")
            {
                if (status == "cho-xac-nhan") query = query.Where(d => d.TrangThaiDonHang == "Chờ xác nhận");
                else if (status == "da-xac-nhan") query = query.Where(d => d.TrangThaiDonHang == "Đã xác nhận");
                else if (status == "dang-chuan-bi") query = query.Where(d => d.TrangThaiDonHang == "Đang chuẩn bị");
                else if (status == "dang-giao") query = query.Where(d => d.TrangThaiDonHang == "Đang giao");
                else if (status == "hoan-thanh") query = query.Where(d => d.TrangThaiDonHang == "Hoàn thành" || d.TrangThaiDonHang == "Đã giao");
                else if (status == "da-huy") query = query.Where(d => d.TrangThaiDonHang == "Đã hủy" || d.TrangThaiDonHang == "Từ chối");
            }

            // Pagination Logic
            const int pageSize = 5;
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var donHangs = await query
                .OrderByDescending(d => d.NgayTaoDon)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            var reviewedItems = await _context.DanhGiaMonAns
                .Where(d => d.MaKH == khachHang.MaKH && d.MaDonHang != null)
                .Select(d => d.MaDonHang.ToString() + "_" + d.MaMonAn.ToString())
                .ToListAsync();
            ViewBag.ReviewedItems = reviewedItems;

            return View(donHangs);
        }

        [HttpPost("huydonmua")]
        public async Task<IActionResult> HuyDonMua(int id, string lyDo)
        {
            if (!CheckIsKhachHang())
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var khachHang = await GetCurrentKhachHang();
            var donHang = await _context.DonHangs.FirstOrDefaultAsync(d => d.MaDonHang == id && d.MaKH == khachHang.MaKH);

            if (donHang == null)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });

            if (donHang.TrangThaiDonHang != "Chờ xác nhận")
                return Json(new { success = false, message = "Chỉ có thể hủy đơn hàng ở trạng thái Chờ xác nhận" });

            donHang.TrangThaiDonHang = "Đã hủy";
            donHang.LyDoHuy = lyDo;
            _context.Update(donHang);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Hủy đơn hàng thành công!" });
        }
    }
}
