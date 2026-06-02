using System;
using System.Collections.Generic;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class TheLoaiMonAn
    {
        public TheLoaiMonAn()
        {
            MonAns = new HashSet<MonAn>();
        }

        public int MaLoaiMon { get; set; }
        public string TenLoai { get; set; } = null!;

        public virtual ICollection<MonAn> MonAns { get; set; }
    }
}
