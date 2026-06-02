import pyodbc

conn_str = 'Driver={ODBC Driver 17 for SQL Server};Server=(localdb)\MSSQLLocalDB;Database=Qly_HTBanHangDoAnThucUong;Trusted_Connection=yes;'

sql = """
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='KhuyenMai' and xtype='U')
BEGIN
    CREATE TABLE KhuyenMai (
        MaKhuyenMai int IDENTITY(1,1) PRIMARY KEY,
        TenKhuyenMai nvarchar(255) NOT NULL,
        MaGiamGia nvarchar(50) NOT NULL,
        PhanTramGiam int NOT NULL,
        DieuKienApDung decimal(18,2) NOT NULL,
        SuDung1Lan bit NOT NULL,
        TrangThai nvarchar(50) NULL,
        MaGianHang int NULL
    );
END
"""

try:
    conn = pyodbc.connect(conn_str)
    cursor = conn.cursor()
    cursor.execute(sql)
    conn.commit()
    print("Table KhuyenMai created successfully!")
except Exception as e:
    print(f"Error: {e}")
