using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public partial class LichSuDonHang
    {
        public int MaLichSu { get; set; }
        public int MaDonHang { get; set; }
        public string TrangThaiDonHang { get; set; } = null!;
        public DateTime? ThoiGian { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
    }
}
