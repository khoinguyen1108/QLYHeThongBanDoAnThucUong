using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangDoAnThucUong.Models.ViewModels
{
    public class NangCapDoiTacViewModel
    {
        // ===== THÔNG TIN GIAN HÀNG =====
        [Required(ErrorMessage = "Tên c?a hàng không du?c d? tr?ng")]
        [Display(Name = "Tên C?a Hàng")]
        public string TenQuanDoiTac { get; set; }

        [Required(ErrorMessage = "Tên gian hàng không du?c d? tr?ng")]
        [Display(Name = "Tên Gian Hàng")]
        public string TenGianHang { get; set; }

        // ===== THÔNG TIN LIÊN H? (T? Ð?NG T? KHÁCH HÀNG) =====
        [Required(ErrorMessage = "S? di?n tho?i không du?c d? tr?ng")]
        [RegularExpression(@"^\d{10,11}$",
            ErrorMessage = "S? di?n tho?i ph?i 10-11 ch? s?")]
        [Display(Name = "S? Ði?n Tho?i")]
        public string SoDTDoiTac { get; set; }

        [Required(ErrorMessage = "Email không du?c d? tr?ng")]
        [EmailAddress(ErrorMessage = "Email không h?p l?")]
        [Display(Name = "Email")]
        public string EmailDTac { get; set; }

        // ===== Ð?A CH? =====
        [Required(ErrorMessage = "Ð?a ch? c?a hàng không du?c d? tr?ng")]
        [Display(Name = "Ð?a Ch? C?a Hàng")]
        public string? DiaChiDoiTac { get; set; }

        [Required(ErrorMessage = "Ð?a ch? gian hàng không du?c d? tr?ng")]
        [Display(Name = "Ð?a Ch? Gian Hàng")]
        public string? DiaChiCuThe { get; set; }

        // ===== GI? HO?T Ð?NG =====
        [Required(ErrorMessage = "Gi? m? c?a không du?c d? tr?ng")]
        [Display(Name = "Gi? M? C?a")]
        public string? GioMo { get; set; }

        [Required(ErrorMessage = "Gi? dóng c?a không du?c d? tr?ng")]
        [Display(Name = "Gi? Ðóng C?a")]
        public string? GioDong { get; set; }

        // ===== ÐI?U L? & XÁC NH?N =====
        [Required(ErrorMessage = "Vui lòng ch?n di?u l?")]
        [Display(Name = "Ch?n Ði?u L?")]
        public int MaDieuLe { get; set; }

        //[Required(ErrorMessage = "Vui lòng xác nh?n dã d?c di?u l?")]
        [Display(Name = "Tôi dã d?c và d?ng ý v?i di?u l?")]
        public bool DaXacNhanDieuLe { get; set; }
    }
}