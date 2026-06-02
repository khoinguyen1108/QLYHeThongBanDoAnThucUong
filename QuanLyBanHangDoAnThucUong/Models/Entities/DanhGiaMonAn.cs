using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    [Table("DanhGiaMonAn")]
    public class DanhGiaMonAn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDanhGia { get; set; }

        public int? MaMonAn { get; set; }

        public int? MaKH { get; set; }

        public int? MaDonHang { get; set; }

        [Required]
        [Range(1, 5)]
        public int SoSao { get; set; }

        [StringLength(500)]
        public string? NoiDung { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string? PhanHoiCuaDoiTac { get; set; }

        public DateTime? NgayPhanHoi { get; set; }

        public bool TrangThaiHienThi { get; set; } = true;

        // Navigation
        [ForeignKey("MaMonAn")]
        public MonAn? MonAn { get; set; }

        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }

        [ForeignKey("MaDonHang")]
        public DonHang? DonHang { get; set; }
    }
}
