using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class ThongTinKhachHangViewModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [Display(Name = "Tên Khách Hàng")]
        public string TenKH { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải 10-11 chữ số")]
        [Display(Name = "Số Điện Thoại")]
        public string SoDienThoai { get; set; }

        [Display(Name = "Giới Tính")]
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string GioiTinh { get; set; }

        [Display(Name = "Thành Phố")]
        [Required(ErrorMessage = "Vui lòng chọn Thành phố")]
        public string ThanhPho { get; set; }

        [Display(Name = "Phường / Xã")]
        [Required(ErrorMessage = "Vui lòng chọn Phường / Xã")]
        public string PhuongXa { get; set; }

        [Display(Name = "Địa Chỉ Cụ Thể (Số nhà, đường...)")]
        public string DiaChi { get; set; }

        // Đổi mật khẩu
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu Cũ")]
        public string MatKhauCu { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật Khẩu Mới")]
        public string MatKhauMoi { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        [Display(Name = "Xác Nhận Mật Khẩu Mới")]
        public string XacNhanMatKhauMoi { get; set; }
    }
}
