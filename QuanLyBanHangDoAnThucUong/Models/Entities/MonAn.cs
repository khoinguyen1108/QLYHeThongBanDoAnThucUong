namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class MonAn
    {
        public int MaMonAn { get; set; }
        public int MaGianHang { get; set; }
        public int MaLoaiMon { get; set; }
        public string? TenMon { get; set; }
        public string? ThanhPhan { get; set; }
        public decimal? SoSaoTrungBinh { get; set; } = 0;
        public int? SoLuotDanhGia { get; set; } = 0;
        public int? SoLuotBan { get; set; } = 0;

        public GianHang? GianHang { get; set; }
        public TheLoaiMonAn? TheLoaiMonAn { get; set; }
        public ICollection<BienTheMonAn> BienTheMonAns { get; set; } = new List<BienTheMonAn>();
    }
}
