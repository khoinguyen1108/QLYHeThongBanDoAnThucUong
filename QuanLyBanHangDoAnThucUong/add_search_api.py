import re

with open('Controllers/HomeController.cs', 'r', encoding='utf-8') as f:
    text = f.read()

# Add SearchSuggestions API before GetDanhGia
insert_str = """
        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) 
                return Json(new { monAn = new List<object>(), gianHang = new List<object>() });

            var queryStr = q.ToLower();

            var monAn = await _context.MonAns
                .Include(m => m.BienTheMonAns)
                .Where(m => m.TrangThaiHienThi && m.TenMon.ToLower().Contains(queryStr))
                .OrderByDescending(m => m.SoLuotDanhGia)
                .Take(5)
                .Select(m => new {
                    id = m.MaMonAn,
                    ten = m.TenMon,
                    hinh = m.BienTheMonAns.FirstOrDefault() != null ? m.BienTheMonAns.FirstOrDefault().HinhAnhMonAn : null,
                    gia = m.BienTheMonAns.FirstOrDefault() != null ? m.BienTheMonAns.FirstOrDefault().GiaBan : 0
                })
                .ToListAsync();

            var gianHang = await _context.GianHangs
                .Where(g => g.TrangThaiHoatDong && g.TenGianHang.ToLower().Contains(queryStr))
                .OrderByDescending(g => g.NgayTao)
                .Take(3)
                .Select(g => new {
                    id = g.MaGianHang,
                    ten = g.TenGianHang,
                    hinh = g.HinhAnh,
                    diaChi = g.DiaChiCuThe
                })
                .ToListAsync();

            return Json(new { monAn, gianHang });
        }
"""

# Insert before public async Task<IActionResult> GetDanhGia
text = text.replace('public async Task<IActionResult> GetDanhGia', insert_str + '\n        public async Task<IActionResult> GetDanhGia')

with open('Controllers/HomeController.cs', 'w', encoding='utf-8') as f:
    f.write(text)

print("Added SearchSuggestions API")
