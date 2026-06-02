using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class DanhGiaMonAn
    {
        public int MaDanhGia { get; set; }
        public int MaMonAn { get; set; }
        public int? MaKh { get; set; }
        public int MaDonHang { get; set; }
        public int SoSao { get; set; }
        public string? NoiDung { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public string? PhanHoiCuaDoiTac { get; set; }
        public DateTime? NgayPhanHoi { get; set; }
        public bool? TrangThaiHienThi { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual MonAn MaMonAnNavigation { get; set; } = null!;
    }
}
