using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class DonHang
    {
        public DonHang()
        {
            CtDonHangs = new HashSet<CtDonHang>();
            DanhGiaMonAns = new HashSet<DanhGiaMonAn>();
            GiaoHangs = new HashSet<GiaoHang>();
            LichSuDonHangs = new HashSet<LichSuDonHang>();
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaDonHang { get; set; }
        public int? MaKh { get; set; }
        public int MaGianHang { get; set; }
        public int? MaKmai { get; set; }
        public DateTime NgayTaoDon { get; set; }
        public decimal TongTienMon { get; set; }
        public decimal GiamGia { get; set; }
        public decimal PhiShip { get; set; }
        public decimal PhiDichVuSan { get; set; }
        public decimal ThanhTienKhachTra { get; set; }
        public decimal ThanhTienQuanNhan { get; set; }
        public string TrangThaiDonHang { get; set; } = null!;
        public string? TrangThaiThanhToan { get; set; }
        public string? LyDoHuy { get; set; }
        public string? GhiChu { get; set; }
        public int? MaDiaChi { get; set; }
        public string TenNguoiNhan { get; set; } = null!;
        public string SoDienThoaiNhan { get; set; } = null!;
        public string DiaChiGiaoHang { get; set; } = null!;
        public decimal ViDoGiao { get; set; }
        public decimal KinhDoGiao { get; set; }

        public virtual DiaChi? MaDiaChiNavigation { get; set; }
        public virtual GianHang MaGianHangNavigation { get; set; } = null!;
        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual ChuongTrinhKhuyenMai? MaKmaiNavigation { get; set; }
        public virtual ICollection<CtDonHang> CtDonHangs { get; set; }
        public virtual ICollection<DanhGiaMonAn> DanhGiaMonAns { get; set; }
        public virtual ICollection<GiaoHang> GiaoHangs { get; set; }
        public virtual ICollection<LichSuDonHang> LichSuDonHangs { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
