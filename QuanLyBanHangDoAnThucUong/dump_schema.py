import pyodbc
import json

conn = pyodbc.connect('Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\MSSQLLocalDB;Database=Qly_HTBanHangDoAnThucUong;Trusted_Connection=yes;')
cursor = conn.cursor()

tables = ['VaiTro', 'TaiKhoan', 'DieuLe', 'DoiTac', 'GianHang', 'MonAn', 'BienTheMonAn', 'TheLoaiMonAn', 'KhachHang', 'DonHang', 'CT_DonHang', 'GioHang', 'CT_GioHang', 'ChuongTrinhKhuyenMai', 'KhuyenMai', 'DanhGiaMonAn', 'LichSuTimKiem']

schema = {}

for table in tables:
    try:
        cursor.execute(f"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'")
        cols = []
        for row in cursor.fetchall():
            cols.append(f"{row.COLUMN_NAME} ({row.DATA_TYPE})")
        schema[table] = cols
    except Exception as e:
        schema[table] = str(e)

with open('schema_dump.json', 'w') as f:
    json.dump(schema, f, indent=4)
