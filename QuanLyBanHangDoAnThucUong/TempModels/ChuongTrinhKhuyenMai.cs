using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class ChuongTrinhKhuyenMai
    {
        public ChuongTrinhKhuyenMai()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int MaKmai { get; set; }
        public string TenChTrinh { get; set; } = null!;
        public string NoiDungKmai { get; set; } = null!;
        public string? LoaiKhuyenMai { get; set; }
        public int? MaGianHang { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public double PhanTramGiam { get; set; }
        public decimal GiamToiDa { get; set; }
        public string? TrangThaiKmai { get; set; }

        public virtual GianHang? MaGianHangNavigation { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
