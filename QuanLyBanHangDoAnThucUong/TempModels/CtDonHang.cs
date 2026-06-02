using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class CtDonHang
    {
        public int MaCtdonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaBienThe { get; set; }
        public int SoLuongMua { get; set; }
        public decimal GiaBan { get; set; }

        public virtual BienTheMonAn MaBienTheNavigation { get; set; } = null!;
        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
    }
}
