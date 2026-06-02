using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class TinNhan
    {
        public int MaTinNhan { get; set; }
        public int? NguoiGui { get; set; }
        public int? NguoiNhan { get; set; }
        public int? MaDanhGia { get; set; }
        public string NoiDung { get; set; } = null!;
        public DateTime ThoiGianGui { get; set; }
        public bool DaXem { get; set; }

        public virtual TaiKhoan? NguoiGuiNavigation { get; set; }
        public virtual TaiKhoan? NguoiNhanNavigation { get; set; }
    }
}
