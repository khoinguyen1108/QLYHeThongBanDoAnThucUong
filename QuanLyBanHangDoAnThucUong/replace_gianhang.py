import re

file_path = r'd:\Khóa Luận Cử Nhân\HeThongBanHang\QuanLyBanHangDoAnThucUong (8)\QuanLyBanHangDoAnThucUong\Controllers\QuanLyGianHangController.cs'

with open(file_path, 'r', encoding='utf-8') as f:
    content = f.read()

# Add helper method after GetCurrentDoiTac()
helper_method = """
        [HttpPost]
        public IActionResult ChangeBranch(int id)
        {
            HttpContext.Session.SetInt32("SelectedGianHangId", id);
            return Json(new { success = true });
        }

        private IQueryable<GianHang> GetGianHangQuery(DoiTac doiTac)
        {
            var selectedBranchId = HttpContext.Session.GetInt32("SelectedGianHangId");
            var q = _context.GianHangs.Where(g => g.MaDoiTac == doiTac.MaDoiTac);
            if (selectedBranchId.HasValue) {
                q = q.Where(g => g.MaGianHang == selectedBranchId.Value);
            }
            return q;
        }
"""

content = content.replace('        // ===== TRANG CHỦ QUẢN LÝ =====', helper_method + '\n        // ===== TRANG CHỦ QUẢN LÝ =====')

# Replace exact strings
content = content.replace('_context.GianHangs\n                .Include(g => g.DieuLe)\n                .Include(g => g.DoiTac)\n                .Include(g => g.DoiTac.MonAns)\n                    .ThenInclude(m => m.BienTheMonAns)\n                .FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac)', 'GetGianHangQuery(doiTac)\n                .Include(g => g.DieuLe)\n                .Include(g => g.DoiTac)\n                .Include(g => g.DoiTac.MonAns)\n                    .ThenInclude(m => m.BienTheMonAns)\n                .FirstOrDefaultAsync()')

content = content.replace('_context.GianHangs.FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac)', 'GetGianHangQuery(doiTac).FirstOrDefaultAsync()')

content = content.replace('_context.GianHangs\n                .FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac)', 'GetGianHangQuery(doiTac).FirstOrDefaultAsync()')

# Some might have include chains
content = content.replace('_context.GianHangs\n                .Include(g => g.DoiTac)\n                .FirstOrDefaultAsync(g => g.MaDoiTac == doiTac.MaDoiTac)', 'GetGianHangQuery(doiTac)\n                .Include(g => g.DoiTac)\n                .FirstOrDefaultAsync()')

# Custom replacements for the exact ones shown in grep
content = re.sub(r'_context\.GianHangs\s*\.FirstOrDefaultAsync\(g => g\.MaDoiTac == doiTac\.MaDoiTac\)', 'GetGianHangQuery(doiTac).FirstOrDefaultAsync()', content)

# I should use regex to catch all `_context.GianHangs(.+?)FirstOrDefaultAsync\(g => g\.MaDoiTac == doiTac\.MaDoiTac\)`
content = re.sub(r'_context\.GianHangs([\s\S]*?)\.FirstOrDefaultAsync\(g => g\.MaDoiTac == doiTac\.MaDoiTac\)', r'GetGianHangQuery(doiTac)\1.FirstOrDefaultAsync()', content)

with open(file_path, 'w', encoding='utf-8') as f:
    f.write(content)

print("Replaced successfully!")
