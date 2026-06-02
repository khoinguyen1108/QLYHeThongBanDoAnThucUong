namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class DoiTac
    {
        public int MaDoiTac { get; set; }
        public string? TenQuanDoiTac { get; set; }
        public string? SoDTDoiTac { get; set; }
        public string? DiaChiDoiTac { get; set; }
        public string? EmailDTac { get; set; }
        public string? TrangThaiDoiTac { get; set; }
        public int MaTaiKhoan { get; set; }

        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<GianHang> GianHangs { get; set; } = new List<GianHang>();
    }
}
