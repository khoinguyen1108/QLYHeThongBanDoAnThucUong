using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (!userId.HasValue)
            {
                return Ok(new { count = 0 });
            }

            var count = await _context.TinNhans
                .CountAsync(t => t.NguoiNhan == userId.Value && !t.DaXem);

            return Ok(new { count = count });
        }
    }
}

