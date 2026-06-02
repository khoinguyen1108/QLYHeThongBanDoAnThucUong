using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            DanhGiaMonAns = new HashSet<DanhGiaMonAn>();
            DiaChis = new HashSet<DiaChi>();
            DonHangs = new HashSet<DonHang>();
            GioHangs = new HashSet<GioHang>();
            LichSuXemMons = new HashSet<LichSuXemMon>();
        }

        public int MaKh { get; set; }
        public string? TenKh { get; set; }
        public string? GioiTinhKh { get; set; }
        public string? SoDtkh { get; set; }
        public string? DiaChiCuThe { get; set; }
        public string? PhuongXa { get; set; }
        public string? ThanhPho { get; set; }
        public int? DiemTichLuy { get; set; }
        public string HangThanhVien { get; set; } = null!;
        public string EmailKh { get; set; } = null!;
        public int? MaTaiKhoan { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string? ThuHangBac { get; set; }
        public string? Avatar { get; set; }

        public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
        public virtual ICollection<DanhGiaMonAn> DanhGiaMonAns { get; set; }
        public virtual ICollection<DiaChi> DiaChis { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<LichSuXemMon> LichSuXemMons { get; set; }
    }
}
