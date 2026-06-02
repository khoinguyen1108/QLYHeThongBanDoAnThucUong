using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class ThemMonAnViewModel
    {
        [Required(ErrorMessage = "Tên món ăn không được để trống")]
        [Display(Name = "Tên Món Ăn")]
        public string? TenMon { get; set; }

        [Required(ErrorMessage = "Loại món không được để trống")]
        [Display(Name = "Loại Món")]
        public int MaLoaiMon { get; set; }

        // ===== BIẾN THỂ MÓN ĂN =====
        //[Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô Tả")]
        public string? MoTaMonAn { get; set; }

        [Required(ErrorMessage = "Giá bán không được để trống")]
        [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1,000 đến 10,000,000")]
        [Display(Name = "Giá Bán")]
        public decimal GiaBan { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(0, 99999, ErrorMessage = "Số lượng phải từ 0 trở lên")]
        [Display(Name = "Số Lượng")]
        public int SoLuongMon { get; set; }

        // ===== CÁC TRƯỜNG KHÔNG BẮT BUỘC =====
        [Display(Name = "Hình Ảnh")]
        public string? HinhAnhMonAn { get; set; }

        public IFormFile? FileHinh { get; set; }

        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Món Ăn Mua Thêm (Tùy chọn kèm)")]
        public string? MonAnMuaThem { get; set; }
    }
}