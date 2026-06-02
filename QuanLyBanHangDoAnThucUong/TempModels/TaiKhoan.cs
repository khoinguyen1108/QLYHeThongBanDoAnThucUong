using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            DoiTacs = new HashSet<DoiTac>();
            KhachHangs = new HashSet<KhachHang>();
            NhanVienHeThongs = new HashSet<NhanVienHeThong>();
            TinNhanNguoiGuiNavigations = new HashSet<TinNhan>();
            TinNhanNguoiNhanNavigations = new HashSet<TinNhan>();
        }

        public int MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public int MaVaiTro { get; set; }
        public string? TrangThai { get; set; }

        public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
        public virtual ICollection<DoiTac> DoiTacs { get; set; }
        public virtual ICollection<KhachHang> KhachHangs { get; set; }
        public virtual ICollection<NhanVienHeThong> NhanVienHeThongs { get; set; }
        public virtual ICollection<TinNhan> TinNhanNguoiGuiNavigations { get; set; }
        public virtual ICollection<TinNhan> TinNhanNguoiNhanNavigations { get; set; }
    }
}
