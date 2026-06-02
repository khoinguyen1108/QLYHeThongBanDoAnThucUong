namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class GioHang
    {
        public int MaGioHang { get; set; }
        public int? MaKH { get; set; }

        public KhachHang? KhachHang { get; set; }
        public ICollection<CT_GioHang> CT_GioHangs { get; set; } = new List<CT_GioHang>();
    }
}
