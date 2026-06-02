using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class GianHang
    {
        public int MaGianHang { get; set; }
        public int MaDoiTac { get; set; }
        public int MaDieuLe { get; set; }
        public string? TenGianHang { get; set; }
        public string? PhuongXa { get; set; }
        public string? ThanhPho { get; set; }
        public string? DiaChiCuThe { get; set; }
        public string? HinhAnh { get; set; }
        public TimeSpan GioMo { get; set; }
        public TimeSpan GioDong { get; set; }
        [Column("DanhGiaGianHang")]
        public string? DanhGiaGianHang { get; set; }

        [NotMapped]
        public decimal? DanhGiaTB 
        {
            get 
            {
                if (string.IsNullOrEmpty(DanhGiaGianHang)) return 0m;
                if (decimal.TryParse(DanhGiaGianHang, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
                return 0m;
            }
            set 
            {
                DanhGiaGianHang = value?.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public int LuotXem { get; set; }
        public string? TrangThaiGianHang { get; set; } = "Tạm ngưng";

        public DoiTac? DoiTac { get; set; }
        public DieuLe? DieuLe { get; set; }
        public ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
    }
}
