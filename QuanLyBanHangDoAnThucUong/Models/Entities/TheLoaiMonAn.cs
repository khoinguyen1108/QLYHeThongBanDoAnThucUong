namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class TheLoaiMonAn
    {
        public int MaLoaiMon { get; set; }
        public string? TenLoai { get; set; }

        public ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
    }
}
