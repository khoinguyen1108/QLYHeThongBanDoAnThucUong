using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class SuaMonAnViewModel
    {
        [Required(ErrorMessage = "Mã món ăn không được để trống")]
        public int MaMonAn { get; set; }

        [Required(ErrorMessage = "Tên món ăn không được để trống")]
        [Display(Name = "Tên Món Ăn")]
        public string TenMon { get; set; }

        [Required(ErrorMessage = "Loại món không được để trống")]
        [Display(Name = "Loại Món")]
        public int MaLoaiMon { get; set; }

        // ===== THÔNG TIN BIẾN THỂ =====
        [Required(ErrorMessage = "Mã biến thể không được để trống")]
        public int MaBienThe { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
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

        [Display(Name = "Hình Ảnh")]
        public string? HinhAnhMonAn { get; set; }

        public IFormFile? FileHinh { get; set; }

        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Món Ăn Mua Thêm (Tùy chọn kèm)")]
        public string? MonAnMuaThem { get; set; }

        [Display(Name = "Trạng Thái")]
        public string? TrangThaiMonAn { get; set; }
    }
}