using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    public class ChuongTrinhKhuyenMai
    {
        [Key]
        public int MaKMai { get; set; }
        public string TenChTrinh { get; set; } = string.Empty;
        public string NoiDungKMai { get; set; } = string.Empty;
        
        [Column("NgayBDau")]
        public DateTime NgayBatDau { get; set; }
        
        [Column("NgayKThuc")]
        public DateTime NgayKetThuc { get; set; }
        public decimal PhanTramGiam { get; set; }
        public decimal GiamToiDa { get; set; }
        public string? LoaiKhuyenMai { get; set; }
        public string? TrangThaiKMai { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal DieuKienApDung { get; set; } = 0;
        
        public bool SuDung1Lan { get; set; } = true;

        // FK trỏ tới GianHang (tên cột trong DB là MaGianHang)
        public int? MaGianHang { get; set; }

        [ForeignKey("MaGianHang")]
        public GianHang? GianHang { get; set; }
    }
}
