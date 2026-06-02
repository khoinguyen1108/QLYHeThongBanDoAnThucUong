using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Tên đăng nhập từ 3-100 ký tự")]
        [Display(Name = "Tên Đăng Nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không trùng khớp")]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [Display(Name = "Tên Khách Hàng")]
        public string TenKH { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$",
            ErrorMessage = "Số điện thoại phải 10-11 chữ số")]
        [Display(Name = "Số Điện Thoại")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Giới tính")]
        [Display(Name = "Giới Tính")]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Thành phố")]
        [Display(Name = "Thành Phố")]
        public string ThanhPho { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Phường / Xã")]
        [Display(Name = "Phường / Xã")]
        public string PhuongXa { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa Chỉ Cụ Thể (Số nhà, đường...)")]
        public string DiaChi { get; set; }
    }
}
