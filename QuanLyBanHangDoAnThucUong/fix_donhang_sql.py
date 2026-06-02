import re

with open('resources/SQLQuery.sql', 'r', encoding='utf-8') as f:
    text = f.read()

# We need to recreate the DonHang table correctly.
# Let's find the CREATE TABLE DonHang section
pattern = re.compile(r'CREATE TABLE DonHang\(\s+FOREIGN KEY \(MaGianHang\)', re.MULTILINE)

fixed_donhang = """CREATE TABLE DonHang(
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NULL,
    MaGianHang INT NOT NULL,
    MaKMai INT NULL,
    NgayTaoDon DATETIME DEFAULT GETDATE() NOT NULL,
    TongTienMon DECIMAL(18,2) DEFAULT 0 NOT NULL,
    GiamGia DECIMAL(18,2) DEFAULT 0 NOT NULL,
    PhiDichVuSan DECIMAL(18,2) DEFAULT 0 NOT NULL,
    PhiShip DECIMAL(18,2) DEFAULT 0 NOT NULL,
    ThanhTienKhachTra DECIMAL(18,2) DEFAULT 0 NOT NULL,
    ThanhTienQuanNhan DECIMAL(18,2) DEFAULT 0 NOT NULL,
    TrangThaiDonHang NVARCHAR(50) NOT NULL DEFAULT N'Chờ xác nhận'
       CHECK (TrangThaiDonHang IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đang chuẩn bị',
                                     N'Đang giao', N'Hoàn thành', N'Đã hủy', N'Từ chối')),
    TrangThaiThanhToan NVARCHAR(50) NOT NULL DEFAULT N'Chưa thanh toán'
       CHECK (TrangThaiThanhToan IN (N'Chưa thanh toán', N'Đã thanh toán')),
    LyDoHuy NVARCHAR(500) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    MaDiaChi INT NULL,
    TenNguoiNhan NVARCHAR(255) NULL,
    SoDienThoaiNhan NVARCHAR(50) NULL,
    DiaChiGiaoHang NVARCHAR(MAX) NULL,
    ViDoGiao DECIMAL(18,6) DEFAULT 0 NOT NULL,
    KinhDoGiao DECIMAL(18,6) DEFAULT 0 NOT NULL,

    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaGianHang)"""

text = text.replace("CREATE TABLE DonHang(\n   FOREIGN KEY (MaGianHang)", fixed_donhang)

with open('resources/SQLQuery.sql', 'w', encoding='utf-8') as f:
    f.write(text)

print("Fixed SQLQuery.sql DonHang schema.")
