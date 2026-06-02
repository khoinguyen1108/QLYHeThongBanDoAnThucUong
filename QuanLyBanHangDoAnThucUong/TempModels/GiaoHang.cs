using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class GiaoHang
    {
        public int MaGiaoHang { get; set; }
        public int MaNv { get; set; }
        public int MaDonHang { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public TimeSpan? TgianXuatPhat { get; set; }
        public TimeSpan? TgianDuKienDen { get; set; }
        public string? TrangThaiGhang { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
        public virtual NhanVienHeThong MaNvNavigation { get; set; } = null!;
    }
}
