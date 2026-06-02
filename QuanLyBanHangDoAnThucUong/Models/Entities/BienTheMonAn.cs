namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class BienTheMonAn
    {
        public int MaBienThe { get; set; }
        public int MaMonAn { get; set; }
        public string? MoTaMonAn { get; set; }
        public int SoLuongMon { get; set; }
        public decimal GiaBan { get; set; }
        public string? HinhAnhMonAn { get; set; }
        public string? TrangThaiMonAn { get; set; } = "Còn bán";
        public string? MonAnMuaThem { get; set; }
        public string? GhiChu { get; set; }

        public MonAn? MonAn { get; set; }
    }
}
