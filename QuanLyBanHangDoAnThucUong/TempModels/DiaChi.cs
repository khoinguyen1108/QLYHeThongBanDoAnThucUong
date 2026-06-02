using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class DiaChi
    {
        public DiaChi()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int MaDiaChi { get; set; }
        public int? MaKh { get; set; }
        public int? MaGianHang { get; set; }
        public string? TenGoiNho { get; set; }
        public string TenNguoiNhan { get; set; } = null!;
        public string SoDienThoaiNhan { get; set; } = null!;
        public string DiaChiCuThe { get; set; } = null!;
        public string PhuongXa { get; set; } = null!;
        public string ThanhPho { get; set; } = null!;
        public decimal ViDo { get; set; }
        public decimal KinhDo { get; set; }
        public bool? LaMacDinh { get; set; }
        public bool? DaXoa { get; set; }

        public virtual GianHang? MaGianHangNavigation { get; set; }
        public virtual KhachHang? MaKhNavigation { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
