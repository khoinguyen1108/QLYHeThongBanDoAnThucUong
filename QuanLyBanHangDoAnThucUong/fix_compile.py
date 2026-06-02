import os

# 1. Fix AccountController
with open("Controllers/AccountController.cs", "r", encoding="utf-8") as f:
    text = f.read()
text = text.replace('ThuHangBac = "Thường"', '')
with open("Controllers/AccountController.cs", "w", encoding="utf-8") as f:
    f.write(text)

# 2. Fix AdminController
with open("Controllers/AdminController.cs", "r", encoding="utf-8") as f:
    text = f.read()
text = text.replace('thongTinThem = $"Hạng: {kh.ThuHangBac} | Điểm: {kh.DiemTichLuy}"', 'thongTinThem = $"Điểm: {kh.DiemTichLuy}"')
with open("Controllers/AdminController.cs", "w", encoding="utf-8") as f:
    f.write(text)

# 3. Fix ApplicationDbContext
with open("Data/ApplicationDbContext.cs", "r", encoding="utf-8") as f:
    lines = f.readlines()
new_lines = [l for l in lines if 'HangThanhVien' not in l and 'ThuHangBac' not in l]
with open("Data/ApplicationDbContext.cs", "w", encoding="utf-8") as f:
    f.writelines(new_lines)

print("Fixed C# compile errors related to HangThanhVien/ThuHangBac")
