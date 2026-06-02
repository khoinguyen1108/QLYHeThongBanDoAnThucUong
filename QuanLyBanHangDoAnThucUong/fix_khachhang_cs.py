with open('Models/Entities/KhachHang.cs', 'r', encoding='utf-8') as f:
    lines = f.readlines()

new_lines = []
for line in lines:
    if 'public string? HangThanhVien' in line or 'public string? ThuHangBac' in line or '[System.ComponentModel.DataAnnotations.Schema.NotMapped]' in line:
        continue
    new_lines.append(line)

with open('Models/Entities/KhachHang.cs', 'w', encoding='utf-8') as f:
    f.writelines(new_lines)

print("Removed HangThanhVien and ThuHangBac from KhachHang.cs")
