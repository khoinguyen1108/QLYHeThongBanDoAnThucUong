using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHangDoAnThucUong.Models.Entities
{
    [Table("TinNhan")]
    public class TinNhan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTinNhan { get; set; }

        public int? NguoiGui { get; set; }
        public int? NguoiNhan { get; set; }
        
        public int? MaDanhGia { get; set; }

        [Required]
        public string NoiDung { get; set; } = string.Empty;

        public DateTime ThoiGianGui { get; set; } = DateTime.Now;

        public bool DaXem { get; set; } = false;

        [ForeignKey("NguoiGui")]
        public virtual TaiKhoan? NguoiGuiNavigation { get; set; }

        [ForeignKey("NguoiNhan")]
        public virtual TaiKhoan? NguoiNhanNavigation { get; set; }
    }
}
