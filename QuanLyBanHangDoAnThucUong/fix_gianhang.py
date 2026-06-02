import os
import re

with open('resources/SQLQuery.sql', 'r', encoding='utf-8') as f:
    sql = f.read()

# 1. Fix CREATE TABLE GianHang
def repl_gianhang_table(match):
    s = match.group(0)
    s = s.replace('DiaChiGianHang NVARCHAR(255) NOT NULL,', 'DiaChiCuThe NVARCHAR(255) NOT NULL,\n      PhuongXa NVARCHAR(100),')
    s = s.replace('DanhGiaTB DECIMAL(3,1) DEFAULT 0,', 'DanhGiaGianHang NVARCHAR(50) DEFAULT NULL,')
    return s

sql = re.sub(r'CREATE TABLE GianHang\s*\((.*?)\);', repl_gianhang_table, sql, flags=re.DOTALL)

# 2. Fix INSERT INTO GianHang
# Original: INSERT INTO GianHang (MaDoiTac, MaDieuLe, TenGianHang, DiaChiGianHang, ThanhPho, HinhAnh, GioMo, GioDong, DanhGiaTB, LuotXem, TrangThaiGianHang)
# We need to change columns to: DiaChiCuThe, PhuongXa, ThanhPho, ..., DanhGiaGianHang
def repl_gianhang_insert(match):
    s = match.group(0)
    s = s.replace('DiaChiGianHang', 'DiaChiCuThe, PhuongXa')
    s = s.replace('DanhGiaTB', 'DanhGiaGianHang')
    return s

sql = re.sub(r'INSERT INTO GianHang \([^)]+\)', repl_gianhang_insert, sql)

# But wait! If we added PhuongXa to columns, we must add a value for PhuongXa in the VALUES clause of GianHang.
# Let's extract the VALUES part for GianHang and patch it.
def repl_gianhang_values(match):
    # match.group(1) is the content of the whole INSERT statement block after VALUES
    val_block = match.group(1)
    # The rows are separated by ),\n or );\n
    # A single row is like: (1, 1, N'Name', N'Address', N'City', 'img.jpg', '07:00:00', '22:00:00', 4.5, 100, N'Open')
    # We need to insert a value for PhuongXa after Address.
    # We can just inject N'Phường trung tâm' as a dummy value after the 4th item.
    
    # We have to parse it properly because strings can have commas.
    # Actually, a simple regex might work because the format is very strict in SQLQuery.sql.
    # N'123 Le Loi', N'TP.HCM' -> N'123 Le Loi', N'Phường Bến Nghé', N'TP.HCM'
    
    # Let's do a quick regex on the lines
    lines = val_block.split('),')
    new_lines = []
    for line in lines:
        if line.strip() == '': continue
        if not line.endswith(')'): line += ')'
        
        # In SQLQuery.sql, the INSERT for GianHang looks like:
        # (1, 1, N'Highlands Coffee - CN Lê Lợi', N'123 Lê Lợi', N'TP.HCM', 'highlands_leloi.jpg', '07:00', '22:00', 4.8, 1500, N'Mở cửa')
        # We want to replace N'123 Lê Lợi', N'TP.HCM' with N'123 Lê Lợi', N'Phường Bến Nghé', N'TP.HCM'
        # And for the 4.8 (DanhGiaTB), we change it to '4.8' (string) or something. Wait, DanhGiaGianHang is NVARCHAR(50), so '4.8' is fine.
        
        parts = line.split(',')
        # parts[0]: (1, parts[1]: 1, parts[2]: N'Name', parts[3]: N'Address'
        # parts[4] is City. We can insert 'Phường' before parts[4].
        # wait, the city might have comma if it's N'TP.HCM'? No, it's just N'TP.HCM'.
        
        # Let's do a regex to find the address and city
        # N'([^']+)',\s*N'([^']+)',\s*'([^']+)' -> This matches Address, City, Image
        line = re.sub(r"(N'[^']+',)\s*(N'[^']+',\s*'[^\.]+\.(?:jpg|png)')", r"\1 N'Phường Bến Nghé', \2", line)
        
        new_lines.append(line)
        
    return "VALUES\n" + ",\n".join(new_lines)

sql = re.sub(r'INSERT INTO GianHang.*?\s+VALUES\s*(.*?);', lambda m: m.group(0).split("VALUES")[0] + repl_gianhang_values(m), sql, flags=re.DOTALL)


# 3. We also need to fix the UPDATE statement at the bottom that syncs DanhGiaGianHang!
# Earlier I added:
# UPDATE GianHang SET DanhGiaGianHang = ( SELECT CAST(ISNULL(AVG(SoSaoTrungBinh), 0) AS DECIMAL(3,1)) ... )
# Wait, DanhGiaGianHang is NVARCHAR(50), so CAST to DECIMAL(3,1) then CAST to NVARCHAR is fine in SQL.
# But I can explicitly cast to NVARCHAR to be safe.
sql = sql.replace(
    'SELECT CAST(ISNULL(AVG(SoSaoTrungBinh), 0) AS DECIMAL(3,1))',
    'SELECT CAST(CAST(ISNULL(AVG(SoSaoTrungBinh), 0) AS DECIMAL(3,1)) AS NVARCHAR(50))'
)

with open('resources/SQLQuery.sql', 'w', encoding='utf-8') as f:
    f.write(sql)
