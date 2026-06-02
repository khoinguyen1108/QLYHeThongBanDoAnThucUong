import re

with open('Controllers/HomeController.cs', 'r', encoding='utf-8') as f:
    text = f.read()

pattern = r'public async Task<IActionResult> GetDanhGia.*?return Json\(new\s*\{\s*reviews,.*?\}\);\s*\}'

replacement = """public async Task<IActionResult> GetDanhGia(int maMonAn, int page = 1, string sort = "newest", int? filterStar = null)
        {
            var maKH = HttpContext.Session.GetInt32("MaKH");
            const int pageSize = 5;

            var query = _context.DanhGiaMonAns
                .Include(d => d.KhachHang)
                .Where(d => d.MaMonAn == maMonAn);

            if (filterStar.HasValue && filterStar > 0)
                query = query.Where(d => d.SoSao == filterStar);

            query = sort switch
            {
                "oldest" => query.OrderBy(d => d.NgayDanhGia),
                "highest" => query.OrderByDescending(d => d.SoSao),
                "lowest" => query.OrderBy(d => d.SoSao),
                _ => query.OrderByDescending(d => d.NgayDanhGia)
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var dbReviews = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    d.MaDanhGia,
                    d.MaKH,
                    d.SoSao,
                    d.NoiDung,
                    d.NgayDanhGia,
                    TenKH = d.KhachHang != null ? d.KhachHang.TenKH : "Ẩn danh",
                    d.PhanHoiCuaDoiTac,
                    d.NgayPhanHoi
                })
                .ToListAsync();

            var reviews = dbReviews.Select(d => new
            {
                d.MaDanhGia,
                d.MaKH,
                d.SoSao,
                d.NoiDung,
                NgayDanhGia = d.NgayDanhGia.ToString("dd/MM/yyyy HH:mm"),
                d.TenKH,
                d.PhanHoiCuaDoiTac,
                IsOwner = (maKH != null && d.MaKH == maKH),
                NgayPhanHoi = d.NgayPhanHoi.HasValue ? d.NgayPhanHoi.Value.ToString("dd/MM/yyyy HH:mm") : null
            }).ToList();

            // Tổng hợp số sao (tính trên TẤT CẢ đánh giá của món ăn, không bị ảnh hưởng bởi filterStar)
            var starSummary = await _context.DanhGiaMonAns
                .Where(d => d.MaMonAn == maMonAn)
                .GroupBy(d => d.SoSao)
                .Select(g => new { Star = g.Key, Count = g.Count() })
                .ToListAsync();
            
            // Total reviews count for the item (unfiltered)
            var totalUnfilteredCount = await _context.DanhGiaMonAns.CountAsync(d => d.MaMonAn == maMonAn);

            return Json(new
            {
                reviews,
                currentPage = page,
                totalPages,
                totalCount, // filtered count
                totalUnfilteredCount, // unfiltered count
                starSummary
            });
        }"""

new_text = re.sub(pattern, replacement, text, flags=re.DOTALL)

with open('Controllers/HomeController.cs', 'w', encoding='utf-8') as f:
    f.write(new_text)

print("Fixed GetDanhGia")
