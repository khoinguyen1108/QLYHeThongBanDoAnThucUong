namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class KhachHang
    {
        public int MaKH { get; set; }
        public string? TenKH { get; set; }
        public string? GioiTinhKH { get; set; }
        public string? SoDTKH { get; set; }
        public string? PhuongXa { get; set; }
        public string? ThanhPho { get; set; }
        public string? DiaChiCuThe { get; set; }
        public int DiemTichLuy { get; set; } = 0;
        public string? EmailKH { get; set; }
        public int? MaTaiKhoan { get; set; }
        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        // Không có cột trong DB — dùng NotMapped để tránh EF query

        public string? Avatar { get; set; }

        public TaiKhoan? TaiKhoan { get; set; }
    }
}
