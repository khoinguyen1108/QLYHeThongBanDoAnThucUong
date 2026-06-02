with open('resources/SQLQuery.sql', 'r', encoding='utf-8') as f:
    lines = f.readlines()

new_lines = []
for line in lines:
    if 'HangThanhVien' in line or 'ThuHangBac' in line:
        continue
    new_lines.append(line)

with open('resources/SQLQuery.sql', 'w', encoding='utf-8') as f:
    f.writelines(new_lines)

print("Removed HangThanhVien and ThuHangBac from SQLQuery.sql")
