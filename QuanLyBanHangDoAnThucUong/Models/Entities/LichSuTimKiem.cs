using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class LichSuTimKiem
    {
        [Key]
        public int MaLSTimKiem { get; set; }

        public int MaKH { get; set; }

        [StringLength(200)]
        public string? TuKhoa { get; set; }

        public DateTime NgayTimKiem { get; set; } = DateTime.Now;

        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }
    }
}
