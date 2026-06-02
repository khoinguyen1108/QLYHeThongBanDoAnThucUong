using Microsoft.AspNetCore.Mvc;
using QuanLyBanHangDoAnThucUong.Data;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    public abstract class BaseClientController : Controller
    {
        protected readonly ApplicationDbContext _context;

        protected BaseClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected int? GetCurrentCustomerId()
        {
            var maKH = HttpContext.Session.GetInt32("MaKH");
            if (maKH != null) return maKH;

            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");
            if (maTaiKhoan != null && vaiTro == "KhachHang")
            {
                var khachHang = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == maTaiKhoan);
                if (khachHang != null)
                {
                    HttpContext.Session.SetInt32("MaKH", khachHang.MaKH);
                    return khachHang.MaKH;
                }
            }
            return null;
        }
    }
}

