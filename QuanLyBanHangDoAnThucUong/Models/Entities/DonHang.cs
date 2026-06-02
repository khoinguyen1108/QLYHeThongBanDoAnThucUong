using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class DonHang
    {
        public int MaDonHang { get; set; }
        public int? MaKH { get; set; }
        public int MaGianHang { get; set; }
        public int? MaKMai { get; set; }
        public DateTime NgayTaoDon { get; set; } = DateTime.Now;
        // Revenue breakdown
        public decimal TongTienMon { get; set; }
        [NotMapped]
        public decimal DaThanhToan { get; set; } = 0;
        public decimal GiamGia { get; set; } = 0;
        public decimal PhiDichVuSan { get; set; } = 0;
        public decimal PhiShip { get; set; } = 0;
        public decimal ThanhTienKhachTra { get; set; }
        public decimal ThanhTienQuanNhan { get; set; }
        public string TrangThaiDonHang { get; set; } = "Chờ xác nhận";
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";
        public string? LyDoHuy { get; set; }
        public string? GhiChu { get; set; }
        public int? MaDiaChi { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SoDienThoaiNhan { get; set; }
        public string? DiaChiGiaoHang { get; set; }
        public decimal ViDoGiao { get; set; }
        public decimal KinhDoGiao { get; set; }

        // Navigation properties
        public KhachHang? KhachHang { get; set; }
        public GianHang? GianHang { get; set; }
        // public ChuongTrinhKhuyenMai? KhuyenMai { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
