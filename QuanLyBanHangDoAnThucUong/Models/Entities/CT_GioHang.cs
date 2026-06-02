namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class CT_GioHang
    {
        public int MaCTGioHang { get; set; }
        public int MaGioHang { get; set; }
        public int MaBienThe { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal UocTinhThanhTien { get; set; }

        public GioHang? GioHang { get; set; }
        public BienTheMonAn? BienTheMonAn { get; set; }
    }
}
