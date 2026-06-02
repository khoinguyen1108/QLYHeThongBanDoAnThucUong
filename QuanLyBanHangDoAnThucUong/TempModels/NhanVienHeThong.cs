using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class NhanVienHeThong
    {
        public NhanVienHeThong()
        {
            GiaoHangs = new HashSet<GiaoHang>();
        }

        public int MaNv { get; set; }
        public string TenNv { get; set; } = null!;
        public string EmailNv { get; set; } = null!;
        public string DiaChiTtru { get; set; } = null!;
        public string? GioiTinhNv { get; set; }
        public string SoDtnv { get; set; } = null!;
        public string ChucVu { get; set; } = null!;
        public string? TrangThaiLamViec { get; set; }
        public int MaTaiKhoan { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
        public virtual ICollection<GiaoHang> GiaoHangs { get; set; }
    }
}
