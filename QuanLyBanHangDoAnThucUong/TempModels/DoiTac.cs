using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class DoiTac
    {
        public DoiTac()
        {
            GianHangs = new HashSet<GianHang>();
        }

        public int MaDoiTac { get; set; }
        public string TenQuanDoiTac { get; set; } = null!;
        public string? SoDtdoiTac { get; set; }
        public string DiaChiDoiTac { get; set; } = null!;
        public string? EmailDtac { get; set; }
        public string? TrangThaiDoiTac { get; set; }
        public int MaTaiKhoan { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
        public virtual ICollection<GianHang> GianHangs { get; set; }
    }
}
