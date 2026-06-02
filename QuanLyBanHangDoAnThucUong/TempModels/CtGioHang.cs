using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class CtGioHang
    {
        public int MaCtgioHang { get; set; }
        public int MaGioHang { get; set; }
        public int MaBienThe { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal UocTinhThanhTien { get; set; }

        public virtual BienTheMonAn MaBienTheNavigation { get; set; } = null!;
        public virtual GioHang MaGioHangNavigation { get; set; } = null!;
    }
}
