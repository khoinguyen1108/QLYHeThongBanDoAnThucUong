using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class GianHang
    {
        public GianHang()
        {
            ChuongTrinhKhuyenMais = new HashSet<ChuongTrinhKhuyenMai>();
            DiaChis = new HashSet<DiaChi>();
            DonHangs = new HashSet<DonHang>();
            MonAns = new HashSet<MonAn>();
        }

        public int MaGianHang { get; set; }
        public int MaDoiTac { get; set; }
        public int MaDieuLe { get; set; }
        public string TenGianHang { get; set; } = null!;
        public string DiaChiCuThe { get; set; } = null!;
        public string? PhuongXa { get; set; }
        public string? ThanhPho { get; set; }
        public string? HinhAnh { get; set; }
        public TimeSpan? GioMo { get; set; }
        public TimeSpan? GioDong { get; set; }
        public string? DanhGiaGianHang { get; set; }
        public int? LuotXem { get; set; }
        public string? TrangThaiGianHang { get; set; }

        public virtual DieuLe MaDieuLeNavigation { get; set; } = null!;
        public virtual DoiTac MaDoiTacNavigation { get; set; } = null!;
        public virtual ICollection<ChuongTrinhKhuyenMai> ChuongTrinhKhuyenMais { get; set; }
        public virtual ICollection<DiaChi> DiaChis { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<MonAn> MonAns { get; set; }
    }
}
