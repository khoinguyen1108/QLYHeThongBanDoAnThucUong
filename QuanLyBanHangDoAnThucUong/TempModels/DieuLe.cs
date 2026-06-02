using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class DieuLe
    {
        public DieuLe()
        {
            GianHangs = new HashSet<GianHang>();
        }

        public int MaDieuLe { get; set; }
        public string TenDieuLe { get; set; } = null!;
        public decimal? PhiChietKhau { get; set; }
        public DateTime? NgayThemDieuLe { get; set; }
        public string? ChiTietDieuLe { get; set; }

        public virtual ICollection<GianHang> GianHangs { get; set; }
    }
}
