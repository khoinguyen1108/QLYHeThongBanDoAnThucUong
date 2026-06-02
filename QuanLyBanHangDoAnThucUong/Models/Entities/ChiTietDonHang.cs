using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaCTDonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaBienThe { get; set; }
        public int SoLuongMua { get; set; }
        public decimal GiaBan { get; set; }

        // Navigation properties
        public DonHang? DonHang { get; set; }
        public BienTheMonAn? BienTheMonAn { get; set; }
    }
}
