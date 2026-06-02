namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class VaiTro
    {
        public int MaVaiTro { get; set; }
        public string? TenVaiTro { get; set; }

        public ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
}
