namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class DieuLe
    {
        public int MaDieuLe { get; set; }
        public string? TenDieuLe { get; set; }
        public decimal PhiChietKhau { get; set; }
        
        public DateTime NgayThemDieuLe { get; set; } = DateTime.Now;
        
        public string? ChiTietDieuLe { get; set; }

        public ICollection<GianHang> GianHangs { get; set; } = new List<GianHang>();
    }
}
