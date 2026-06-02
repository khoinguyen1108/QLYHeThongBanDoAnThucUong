using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class DangKyDoiTacViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3-100 ký tự")]
        [Display(Name = "Tên Đăng Nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không trùng khớp")]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "Tên Quán Đối Tác không được để trống")]
        [Display(Name = "Tên Quán Đối Tác")]
        public string TenQuanDoiTac { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string EmailDTac { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải 10-11 chữ số")]
        [Display(Name = "Số Điện Thoại")]
        public string SoDTDoiTac { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Thành phố")]
        [Display(Name = "Thành Phố")]
        public string ThanhPho { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Phường / Xã")]
        [Display(Name = "Phường / Xã")]
        public string PhuongXa { get; set; }

        [Required(ErrorMessage = "Địa chỉ đối tác không được để trống")]
        [Display(Name = "Địa Chỉ Cụ Thể (Số nhà, đường...)")]
        public string DiaChiDoiTac { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn điều lệ")]
        public int MaDieuLe { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với điều lệ hệ thống")]
        public bool DongYDieuLe { get; set; }
    }
}
