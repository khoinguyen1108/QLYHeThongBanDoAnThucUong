using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public partial class LichSuXemMon
    {
        public int Ma { get; set; }
        public int? MaKh { get; set; }
        public int? MaMonAn { get; set; }
        public DateTime? ThoiGian { get; set; }

        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual MonAn? MaMonAnNavigation { get; set; }
    }
}
