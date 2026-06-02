namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class TaiKhoan
    {
        public int MaTaiKhoan { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public int MaVaiTro { get; set; }
        public string TrangThai { get; set; } = "Hoạt động";
        public string? LyDoHuy { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public VaiTro? VaiTro { get; set; }
        public DoiTac? DoiTac { get; set; }
        public KhachHang? KhachHang { get; set; }
    }
}
