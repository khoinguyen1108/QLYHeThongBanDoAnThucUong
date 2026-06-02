using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class BienTheMonAn
    {
        public BienTheMonAn()
        {
            CtDonHangs = new HashSet<CtDonHang>();
            CtGioHangs = new HashSet<CtGioHang>();
        }

        public int MaBienThe { get; set; }
        public int MaMonAn { get; set; }
        public string MoTaMonAn { get; set; } = null!;
        public int? SoLuongMon { get; set; }
        public decimal GiaBan { get; set; }
        public string? HinhAnhMonAn { get; set; }
        public string? TrangThaiMonAn { get; set; }
        public string? MonAnMuaThem { get; set; }
        public string? GhiChu { get; set; }

        public virtual MonAn MaMonAnNavigation { get; set; } = null!;
        public virtual ICollection<CtDonHang> CtDonHangs { get; set; }
        public virtual ICollection<CtGioHang> CtGioHangs { get; set; }
    }
}
