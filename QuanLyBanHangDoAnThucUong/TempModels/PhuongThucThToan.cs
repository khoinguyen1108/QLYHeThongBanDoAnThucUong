using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class PhuongThucThToan
    {
        public PhuongThucThToan()
        {
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaPtthanhToan { get; set; }
        public string TenPhuongThuc { get; set; } = null!;

        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
