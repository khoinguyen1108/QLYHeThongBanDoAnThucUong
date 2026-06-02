using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;
using System.Threading.Tasks;

namespace QuanLyBanHangDoAnThucUong.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class VNPayWebhookRequest
        {
            public string vnp_TxnRef { get; set; } = string.Empty;
            public string vnp_ResponseCode { get; set; } = string.Empty;
            public string vnp_TransactionNo { get; set; } = string.Empty;
            // Các trường khác của VNPay có thể bỏ qua để đơn giản hoá
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] VNPayWebhookRequest request)
        {
            if (string.IsNullOrEmpty(request.vnp_TxnRef) || string.IsNullOrEmpty(request.vnp_ResponseCode))
            {
                return BadRequest(new { RspCode = "99", Message = "Missing required parameters" });
            }

            // vnp_TxnRef thường được gán là MaDonHang dạng chuỗi
            if (!int.TryParse(request.vnp_TxnRef, out int maDonHang))
            {
                return BadRequest(new { RspCode = "01", Message = "Order not found" });
            }

            var donHang = await _context.DonHangs.FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
            if (donHang == null)
            {
                return BadRequest(new { RspCode = "01", Message = "Order not found" });
            }

            // Mã "00" của VNPay đại diện cho thanh toán thành công
            if (request.vnp_ResponseCode == "00")
            {
                if (donHang.TrangThaiThanhToan != "Đã thanh toán")
                {
                    donHang.TrangThaiThanhToan = "Đã thanh toán";
                    donHang.TrangThaiDonHang = "Đã xác nhận";
                    await _context.SaveChangesAsync();
                }
                return Ok(new { RspCode = "00", Message = "Confirm Success" });
            }
            else
            {
                // Thanh toán thất bại hoặc huỷ
                donHang.TrangThaiThanhToan = "Thanh toán thất bại";
                donHang.TrangThaiDonHang = "Đã huỷ";
                await _context.SaveChangesAsync();
                
                return Ok(new { RspCode = "00", Message = "Confirm Failed Payment" });
            }
        }
    }
}


