using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class MonAn
    {
        public MonAn()
        {
            BienTheMonAns = new HashSet<BienTheMonAn>();
            DanhGiaMonAns = new HashSet<DanhGiaMonAn>();
            LichSuXemMons = new HashSet<LichSuXemMon>();
        }

        public int MaMonAn { get; set; }
        public int MaGianHang { get; set; }
        public int MaLoaiMon { get; set; }
        public string TenMon { get; set; } = null!;
        public string? ThanhPhan { get; set; }
        public decimal? SoSaoTrungBinh { get; set; }
        public int? SoLuotDanhGia { get; set; }
        public int? SoLuotBan { get; set; }

        public virtual GianHang MaGianHangNavigation { get; set; } = null!;
        public virtual TheLoaiMonAn MaLoaiMonNavigation { get; set; } = null!;
        public virtual ICollection<BienTheMonAn> BienTheMonAns { get; set; }
        public virtual ICollection<DanhGiaMonAn> DanhGiaMonAns { get; set; }
        public virtual ICollection<LichSuXemMon> LichSuXemMons { get; set; }
    }
}
