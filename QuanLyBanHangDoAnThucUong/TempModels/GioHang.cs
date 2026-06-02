using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class GioHang
    {
        public GioHang()
        {
            CtGioHangs = new HashSet<CtGioHang>();
        }

        public int MaGioHang { get; set; }
        public int? MaKh { get; set; }

        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual ICollection<CtGioHang> CtGioHangs { get; set; }
    }
}
