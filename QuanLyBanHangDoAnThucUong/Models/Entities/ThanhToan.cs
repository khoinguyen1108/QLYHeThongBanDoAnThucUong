using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public partial class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int MaDonHang { get; set; }
        public int MaPtthanhToan { get; set; }
        public decimal SoTienThanhToan { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string TrangThaiThanhToan { get; set; } = null!;
        public string? GhiChuThanhToan { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
        public virtual PhuongThucThToan MaPtthanhToanNavigation { get; set; } = null!;
    }
}
