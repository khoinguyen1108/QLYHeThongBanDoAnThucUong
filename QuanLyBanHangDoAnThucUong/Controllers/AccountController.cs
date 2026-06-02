using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using QuanLyBanHangDoAnThucUong.Models.Entities;
using QuanLyBanHangDoAnThucUong.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================== ĐĂNG NHẬP ====================

        [HttpGet]
        public IActionResult DangNhap()
        {
            return RedirectToAction("BuyerDangNhap");
        }

        [HttpGet("/buyer/DangNhap")]
        public IActionResult BuyerDangNhap()
        {
            ViewBag.IsSeller = false;
            return View("DangNhap");
        }

        [HttpGet("/seller/DangNhap")]
        public IActionResult SellerDangNhap()
        {
            ViewBag.IsSeller = true;
            return View("DangNhap");
        }

        [HttpPost("/buyer/DangNhap")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyerDangNhap(DangNhapViewModel model)
        {
            ViewBag.IsSeller = false;
            return await ProcessDangNhap(model, "KhachHang");
        }

        [HttpPost("/seller/DangNhap")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerDangNhap(DangNhapViewModel model)
        {
            ViewBag.IsSeller = true;
            return await ProcessDangNhap(model, "DoiTac");
        }

        private async Task<IActionResult> ProcessDangNhap(DangNhapViewModel model, string expectedRole)
        {
            if (!ModelState.IsValid)
                return View("DangNhap", model);

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.VaiTro)
                .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);

            if (taiKhoan == null || !VerifyPassword(model.MatKhau, taiKhoan.MatKhau))
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác");
                return View("DangNhap", model);
            }

            if (taiKhoan.TrangThai == "Bị khóa")
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa");
                return View("DangNhap", model);
            }

            if (taiKhoan.VaiTro.TenVaiTro != expectedRole && taiKhoan.VaiTro.TenVaiTro != "Admin")
            {
                ModelState.AddModelError("", $"Tài khoản này không phải là {expectedRole}");
                return View("DangNhap", model);
            }

            // Lưu vào Session
            HttpContext.Session.SetInt32("MaTaiKhoan", taiKhoan.MaTaiKhoan);
            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro.TenVaiTro);

            if (taiKhoan.VaiTro.TenVaiTro == "KhachHang")
            {
                var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTaiKhoan == taiKhoan.MaTaiKhoan);
                if (khachHang != null)
                {
                    HttpContext.Session.SetInt32("MaKH", khachHang.MaKH);
                }
            }

            // Điều hướng theo vai trò
            if (taiKhoan.VaiTro.TenVaiTro == "DoiTac")
                return RedirectToAction("Index", "QuanLyGianHang");
            else if (taiKhoan.VaiTro.TenVaiTro == "Admin")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Home");
        }

                private async Task PopulateViewBags(bool isSeller)
        {
            ViewBag.IsSeller = isSeller;
            var defaultPhuongXas = new List<string> {
                "Phường Tân Định", "Phường Bến Nghé", "Phường Bến Thành", "Phường Nguyễn Thái Bình",
                "Phường Phạm Ngũ Lão", "Phường Đa Kao", "Phường Cô Giang", "Phường Cầu Ông Lãnh", "Phường Cầu Kho",
                "Phường 1", "Phường 2", "Phường 3", "Phường 4", "Phường 5", "Phường 6", "Phường 7", "Phường 8"
            };
            var dbPhuongXas = await _context.DiaChis
                .Where(d => !string.IsNullOrEmpty(d.PhuongXa))
                .Select(d => d.PhuongXa)
                .Distinct()
                .ToListAsync();
            ViewBag.PhuongXas = defaultPhuongXas.Union(dbPhuongXas).OrderBy(p => p).ToList();

            if (isSeller)
            {
                ViewBag.DieuLes = await _context.DieuLes.ToListAsync();
            }
        }

        // ==================== ĐĂNG KÝ ====================

        [HttpGet]
        public IActionResult DangKy()
        {
            return RedirectToAction("BuyerDangKy");
        }

        [HttpGet("/buyer/DangKy")]
        public async Task<IActionResult> BuyerDangKy()
        {
            await PopulateViewBags(false);
            return View("DangKy");
        }

        [HttpGet("/seller/DangKy")]
        public async Task<IActionResult> SellerDangKy()
        {
            await PopulateViewBags(true);
            return View("DangKyDoiTac");
        }

        [HttpPost("/buyer/DangKy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyerDangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBags(false);
                return View("DangKy", model);
            }

            var existingAccount = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);
            if (existingAccount != null)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                await PopulateViewBags(false);
                return View("DangKy", model);
            }

            var vaiTro = await _context.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == "KhachHang");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = HashPassword(model.MatKhau),
                MaVaiTro = vaiTro.MaVaiTro,
                TrangThai = "Hoạt động"
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            var khachHang = new KhachHang
            {
                TenKH = model.TenKH,
                EmailKH = model.Email,
                SoDTKH = model.SoDienThoai,
                DiaChiCuThe = model.DiaChi,
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                GioiTinhKH = model.GioiTinh,
                ThanhPho = model.ThanhPho,
                PhuongXa = model.PhuongXa
            };

            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký Khách hàng thành công! Vui lòng đăng nhập.";
            return RedirectToAction("BuyerDangNhap");
        }

        [HttpPost("/seller/DangKy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerDangKy(DangKyDoiTacViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBags(true);
                return View("DangKyDoiTac", model);
            }

            var existingAccount = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);
            if (existingAccount != null)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                await PopulateViewBags(true);
                return View("DangKyDoiTac", model);
            }

            var vaiTro = await _context.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == "DoiTac");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = HashPassword(model.MatKhau),
                MaVaiTro = vaiTro.MaVaiTro,
                TrangThai = "Hoạt động"
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            var doiTac = new DoiTac
            {
                TenQuanDoiTac = model.TenQuanDoiTac,
                SoDTDoiTac = model.SoDTDoiTac,
                DiaChiDoiTac = model.DiaChiDoiTac,
                EmailDTac = model.EmailDTac,
                TrangThaiDoiTac = "Chưa đủ điều kiện mở gian hàng",
                MaTaiKhoan = taiKhoan.MaTaiKhoan
            };

            _context.DoiTacs.Add(doiTac);
            await _context.SaveChangesAsync();

            // Khởi tạo GianHang mặc định
            var gianHang = new GianHang
            {
                MaDoiTac = doiTac.MaDoiTac,
                MaDieuLe = model.MaDieuLe,
                TenGianHang = model.TenQuanDoiTac,
                DiaChiCuThe = model.DiaChiDoiTac,
                GioMo = new TimeSpan(7, 0, 0),
                GioDong = new TimeSpan(22, 0, 0),
                TrangThaiGianHang = "Tạm ngưng",
                ThanhPho = model.ThanhPho,
                PhuongXa = model.PhuongXa
            };
            
            _context.GianHangs.Add(gianHang);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký Đối tác thành công! Vui lòng đăng nhập.";
            return RedirectToAction("SellerDangNhap");
        }

        // ==================== ĐĂNG XUẤT ====================
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ==================== HỖ TRỢ ====================
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Trong thực tế, hashOfInput = HashPassword(password)
            // Tạm thời giữ nguyên logic verify cũ để không làm hỏng dữ liệu mẫu
            return password == hash || HashPassword(password) == hash;
        }
    }
}


