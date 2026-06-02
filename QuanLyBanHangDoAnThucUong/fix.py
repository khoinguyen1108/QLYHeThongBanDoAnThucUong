import codecs

file_path = r'd:\Khóa Luận Cử Nhân\HeThongBanHang\QuanLyBanHangDoAnThucUong (8)\QuanLyBanHangDoAnThucUong\Controllers\KhachHangController.cs'
with codecs.open(file_path, 'r', 'utf-8-sig') as f:
    text = f.read()

bad_block = '''        // ===== KIỂM TRA QUYỀN =====
        private bool CheckIsKhachHang()
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            if (!ModelState.IsValid)
                return View(model);'''

good_block = '''        // ===== KIỂM TRA QUYỀN =====
        private bool CheckIsKhachHang()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            return maTaiKhoan.HasValue && vaiTro == "KhachHang";
        }

        private async Task<KhachHang> GetCurrentKhachHang()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            if (!maTaiKhoan.HasValue)
                return null;

            return await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan.Value);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null)
                return RedirectToAction("BuyerDangNhap", "Account");

            return View(khachHang);
        }

        // ===== THÔNG TIN KHÁCH HÀNG =====
        [HttpGet("chinh-sua-ho-so")]
        public async Task<IActionResult> ThongTinKhachHang()
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            var khachHang = await GetCurrentKhachHang();
            if (khachHang == null) return NotFound();

            var model = new ThongTinKhachHangViewModel
            {
                TenKH = khachHang.TenKH,
                Email = khachHang.EmailKH,
                SoDienThoai = khachHang.SoDTKH,
                DiaChi = khachHang.DiaChiCuThe
            };

            return View(model);
        }

        [HttpPost("chinh-sua-ho-so")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThongTinKhachHang(ThongTinKhachHangViewModel model)
        {
            if (!CheckIsKhachHang())
                return RedirectToAction("BuyerDangNhap", "Account");

            if (!ModelState.IsValid)
                return View(model);'''

text = text.replace(bad_block.replace('\n', '\r\n'), good_block.replace('\n', '\r\n'))

with codecs.open(file_path, 'w', 'utf-8-sig') as f:
    f.write(text)
print('Restore success')
