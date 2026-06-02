with open("Views/Admin/QuanLyKhachHang.cshtml", "r", encoding="utf-8") as f:
    text = f.read()

text = text.replace(
    '<span class="badge bg-@GetBadgeColorByRank(kh.ThuHangBac) rounded-pill" style="font-size:0.65rem;">@kh.ThuHangBac</span>',
    '<span class="badge bg-primary rounded-pill" style="font-size:0.65rem;">@kh.DiemTichLuy điểm</span>'
)

# Remove the @functions block
import re
text = re.sub(r'@functions\s*\{\s*private string GetBadgeColorByRank\(string rank\)\s*\{.*?\n\s*\}\s*\}', '', text, flags=re.DOTALL)

with open("Views/Admin/QuanLyKhachHang.cshtml", "w", encoding="utf-8") as f:
    f.write(text)

print("Fixed Views/Admin/QuanLyKhachHang.cshtml")
