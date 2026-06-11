DROP DATABASE Qly_HTBanHangDoAnThucUong;


/* =========================
   TẠO DATABASE
========================= */
USE master;
GO

CREATE DATABASE Qly_HTBanHangDoAnThucUong;
GO

USE Qly_HTBanHangDoAnThucUong;
GO


/* =========================
   BẢNG LOẠI MÓN
========================= */
CREATE TABLE TheLoaiMonAn(
   MaLoaiMon INT IDENTITY(1,1) PRIMARY KEY,
   TenLoai NVARCHAR(100) NOT NULL
);
GO


/* =========================
   Vai Trò
========================= */
CREATE TABLE VaiTro(
   MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
   TenVaiTro NVARCHAR(100) NOT NULL
      CHECK (TenVaiTro IN (N'KhachHang', N'Admin', N'NhanVien', N'DoiTac'))
);
GO


/* =========================
   Tài Khoản
========================= */
CREATE TABLE TaiKhoan(
   MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
   TenDangNhap NVARCHAR(100) UNIQUE NOT NULL,
   MatKhau NVARCHAR(255) NOT NULL,
   MaVaiTro INT NOT NULL,
   TrangThai NVARCHAR(50) DEFAULT N'Hoạt động'
      CHECK (TrangThai IN (N'Hoạt động', N'Bị khóa')),
   LyDoHuy NVARCHAR(MAX) NULL,
   NgayTao DATETIME DEFAULT GETDATE(),
   FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO


/* =========================
   ĐỐI TÁC
========================= */
CREATE TABLE DoiTac(
   MaDoiTac INT IDENTITY(1,1) PRIMARY KEY,
   TenQuanDoiTac NVARCHAR(100) NOT NULL,
   SoDTDoiTac VARCHAR(11) CHECK (LEN(SoDTDoiTac) BETWEEN 10 AND 11 AND SoDTDoiTac NOT LIKE '%[^0-9]%'),
   DiaChiDoiTac NVARCHAR(255) NOT NULL,
   EmailDTac NVARCHAR(255),
   TrangThaiDoiTac NVARCHAR(80) CHECK (TrangThaiDoiTac IN (N'Đã nghỉ', N'Còn đang hoạt động', N'Chưa đủ điều kiện mở gian hàng')),
   MaTaiKhoan INT NOT NULL,
   FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO


/* =========================
   ĐIỀU LỆ
========================= */
CREATE TABLE DieuLe(
    MaDieuLe INT IDENTITY(1,1) PRIMARY KEY,
    TenDieuLe NVARCHAR(200) NOT NULL,
    PhiChietKhau DECIMAL(5,2),
    NgayThemDieuLe DATETIME DEFAULT GETDATE(),
    ChiTietDieuLe NVARCHAR(500)
);
GO


/* =========================
   GIAN HÀNG
========================= */
CREATE TABLE GianHang(
    MaGianHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDoiTac INT NOT NULL,
    MaDieuLe INT NOT NULL,
    TenGianHang NVARCHAR(200) NOT NULL,
    DiaChiCuThe NVARCHAR(255) NOT NULL,
      PhuongXa NVARCHAR(100),
    ThanhPho NVARCHAR(255),  -- Thay doi tu Phuong -> ThanhPho
    HinhAnh NVARCHAR(255),
    GioMo TIME,
    GioDong TIME,
    DanhGiaGianHang NVARCHAR(50) DEFAULT NULL,
    LuotXem INT DEFAULT 0,
    TrangThaiGianHang NVARCHAR(20) CHECK (TrangThaiGianHang IN (N'Mở cửa', N'Đã đóng', N'Tạm ngưng')),
    CONSTRAINT GioHoatDong CHECK (GioDong > GioMo),
    FOREIGN KEY (MaDoiTac) REFERENCES DoiTac(MaDoiTac),
    FOREIGN KEY (MaDieuLe) REFERENCES DieuLe(MaDieuLe)
);
GO


/* =========================
   MÓN ĂN & BIẾN THỂ
========================= */
CREATE TABLE MonAn(
    MaMonAn INT IDENTITY(1,1) PRIMARY KEY,
    MaGianHang INT NOT NULL,
    MaLoaiMon INT NOT NULL,
    TenMon NVARCHAR(200) NOT NULL,
    ThanhPhan NVARCHAR(500),
    SoSaoTrungBinh DECIMAL(3,1) DEFAULT 0,
    SoLuotDanhGia INT DEFAULT 0,
    SoLuotBan INT DEFAULT 0,
    FOREIGN KEY (MaGianHang) REFERENCES GianHang(MaGianHang),
    FOREIGN KEY (MaLoaiMon) REFERENCES TheLoaiMonAn(MaLoaiMon)
);
GO


CREATE TABLE BienTheMonAn(
    MaBienThe INT IDENTITY(1,1) PRIMARY KEY,
    MaMonAn INT NOT NULL,
    MoTaMonAn NVARCHAR(200) NOT NULL,
    SoLuongMon INT DEFAULT 0,
    GiaBan DECIMAL(10,2) NOT NULL,
    HinhAnhMonAn NVARCHAR(255) NULL,
    TrangThaiMonAn NVARCHAR(20) CHECK (TrangThaiMonAn IN (N'Còn bán', N'Món mới', N'Đã hết')),
    MonAnMuaThem NVARCHAR(100),
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn)
);
GO


/* =========================
   KHÁCH HÀNG
========================= */
CREATE TABLE KhachHang(
   MaKH INT IDENTITY(1,1) PRIMARY KEY,
   TenKH NVARCHAR(100),
   GioiTinhKH NCHAR(10) CHECK (GioiTinhKH IN (N'Nam', N'Nữ')),
   SoDTKH VARCHAR(11) CHECK (LEN(SoDTKH) BETWEEN 10 AND 11 AND SoDTKH NOT LIKE '%[^0-9]%'),
   PhuongXa NVARCHAR(255),
   ThanhPho NVARCHAR(255),
   DiaChiCuThe NVARCHAR(255),
   DiemTichLuy INT DEFAULT 0,
   EmailKH NVARCHAR(255) NOT NULL,
   Avatar NVARCHAR(255) NULL,
   MaTaiKhoan INT NULL,
   NgayDangKy DATETIME DEFAULT GETDATE(),
   FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO


ALTER TABLE KhachHang
ADD CONSTRAINT CK_SoDTKH
CHECK (LEN(SoDTKH) BETWEEN 10 AND 11
       AND SoDTKH NOT LIKE '%[^0-9]%');
GO


/* =========================
   NHÂN VIÊN HỆ THỐNG
========================= */
/* =========================
   KHUYẾN MÃI
========================= */
CREATE TABLE ChuongTrinhKhuyenMai(
   MaKMai INT IDENTITY(1,1) PRIMARY KEY,
   TenChTrinh NVARCHAR(100) NOT NULL,
   NoiDungKMai NVARCHAR(200) NOT NULL,
   NgayBDau DATETIME NOT NULL,
   NgayKThuc DATETIME NOT NULL,
   PhanTramGiam DECIMAL(5,2) NOT NULL CHECK (PhanTramGiam > 0 AND PhanTramGiam <= 100),
   GiamToiDa DECIMAL(10,2) NULL,
   DieuKienApDung DECIMAL(10,2) NULL,
   SuDung1Lan BIT DEFAULT 1,
   LoaiKhuyenMai NVARCHAR(100) NULL,
   TrangThaiKMai NVARCHAR(200) CHECK (TrangThaiKMai IN (N'Còn đang trong thời gian', N'Đã hết thời gian sử dụng', N'Mới nhất', N'Kích hoạt')),
   MaGianHang INT NULL,
   CONSTRAINT GioiHanNgay CHECK (NgayKThuc > NgayBDau),
   FOREIGN KEY (MaGianHang) REFERENCES GianHang(MaGianHang) ON DELETE SET NULL
);
GO


/* =========================
   GIỎ HÀNG
========================= */
CREATE TABLE GioHang(
   MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
   MaKH INT NULL,   -- NULL cho phép khách guest
   FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO


CREATE TABLE CT_GioHang(
   MaCTGioHang INT IDENTITY(1,1) PRIMARY KEY,
   MaGioHang INT NOT NULL,
   MaBienThe INT NOT NULL,
   SoLuong INT NOT NULL,
   GiaBan DECIMAL(10,2) NOT NULL,
   UocTinhThanhTien DECIMAL(10,2) NOT NULL,
   FOREIGN KEY (MaGioHang) REFERENCES GioHang(MaGioHang),
   FOREIGN KEY (MaBienThe) REFERENCES BienTheMonAn(MaBienThe)
);
GO


/* =========================
   LỊCH SỬ XEM MÓN
========================= */
/* =========================
   KHUYẾN MÃI (Voucher hệ thống)
========================= */
/* =========================
   PHƯƠNG THỨC THANH TOÁN
========================= */
/* =========================
   ĐƠN HÀNG
========================= */

/* =========================
   ĐỊA CHỈ, LỊCH SỬ TÌM KIẾM, TIN NHẮN
========================= */
CREATE TABLE DiaChi(
    MaDiaChi INT IDENTITY(1,1) PRIMARY KEY,
    MaKh INT NULL,
    MaGianHang INT NULL,
    TenGoiNho NVARCHAR(MAX),
    TenNguoiNhan NVARCHAR(100) NOT NULL,
    SoDienThoaiNhan VARCHAR(11) NOT NULL,
    DiaChiCuThe NVARCHAR(255) NOT NULL,
    PhuongXa NVARCHAR(100) NOT NULL,
    ThanhPho NVARCHAR(100) NOT NULL,
    ViDo DECIMAL(9,6),
    KinhDo DECIMAL(9,6),
    LaMacDinh BIT,
    DaXoa BIT,
    FOREIGN KEY (MaKh) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaGianHang) REFERENCES GianHang(MaGianHang)
);
GO


CREATE TABLE LichSuTimKiem(
    MaLSTimKiem INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NOT NULL,
    TuKhoa NVARCHAR(200),
    NgayTimKiem DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE CASCADE
);
GO


CREATE TABLE TinNhan(
    MaTinNhan INT IDENTITY(1,1) PRIMARY KEY,
    NguoiGui INT NULL,
    NguoiNhan INT NULL,
    MaDanhGia INT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    ThoiGianGui DATETIME DEFAULT GETDATE(),
    DaXem BIT DEFAULT 0,
    FOREIGN KEY (NguoiGui) REFERENCES TaiKhoan(MaTaiKhoan),
    FOREIGN KEY (NguoiNhan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO



CREATE TABLE DonHang(
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
    PhuongThucThanhToan NVARCHAR(50) DEFAULT N'Tiền mặt',
    LyDoHuy NVARCHAR(500) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    MaDiaChi INT NULL,
    TenNguoiNhan NVARCHAR(255) NULL,
    SoDienThoaiNhan NVARCHAR(50) NULL,
    DiaChiGiaoHang NVARCHAR(MAX) NULL,
    ViDoGiao DECIMAL(18,6) DEFAULT 0 NOT NULL,
    KinhDoGiao DECIMAL(18,6) DEFAULT 0 NOT NULL,

    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaGianHang) REFERENCES GianHang(MaGianHang),
   FOREIGN KEY (MaKMai) REFERENCES ChuongTrinhKhuyenMai(MaKMai)
);
GO




CREATE TABLE CT_DonHang (
    MaCTDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    MaBienThe INT NOT NULL,
    SoLuongMua INT NOT NULL,
    GiaBan DECIMAL(18,2) NOT NULL,
    UocTinhThanhTien AS (SoLuongMua * GiaBan),
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    FOREIGN KEY (MaBienThe) REFERENCES BienTheMonAn(MaBienThe)
);
GO

CREATE TABLE NhanVienHeThong (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100),
    MaTaiKhoan INT,
    SoDTNV VARCHAR(15),
    TrangThai NVARCHAR(50),
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO
CREATE TABLE GiaoHang (
    MaGiaoHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT NOT NULL,
    MaDonHang INT NOT NULL,
    PhiVanChuyen DECIMAL(18,2) NOT NULL,
    TgianXuatPhat TIME,
    TgianDuKienDen TIME,
    TrangThaiGhang NVARCHAR(100),
    FOREIGN KEY (MaNV) REFERENCES NhanVienHeThong(MaNV),
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);
GO

INSERT INTO NhanVienHeThong (TenNV, TrangThai) VALUES
(N'Nguyễn Văn A', N'Hoạt động'),
(N'Trần Văn B', N'Hoạt động'),
(N'Lê Văn C', N'Hoạt động'),
(N'Phạm Văn D', N'Hoạt động'),
(N'Hoàng Văn E', N'Hoạt động');

CREATE TABLE DanhGiaMonAn(


    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaMonAn INT NOT NULL,
    MaKH INT NULL,
    MaDonHang INT NULL,
    SoSao INT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(500),
    NgayDanhGia DATETIME DEFAULT GETDATE(),
    PhanHoiCuaDoiTac NVARCHAR(1000),
    NgayPhanHoi DATETIME,
    TrangThaiHienThi BIT DEFAULT 1,
    FOREIGN KEY (MaMonAn) REFERENCES MonAn(MaMonAn),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);
GO

CREATE TABLE PhuongThucThToan (
    MaPTThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    TenPhuongThuc NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE ThanhToan (
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    MaPTThanhToan INT NOT NULL,
    SoTienThanhToan DECIMAL(18,2) NOT NULL,
    NgayThanhToan DATETIME,
    TrangThaiThanhToan NVARCHAR(50) NOT NULL,
    GhiChuThanhToan NVARCHAR(255),
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    FOREIGN KEY (MaPTThanhToan) REFERENCES PhuongThucThToan(MaPTThanhToan)
);
GO

GO
CREATE TRIGGER trg_UpdateDanhGiaMonAn
ON DanhGiaMonAn
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật cho bảng MonAn
    UPDATE MonAn
    SET
        SoLuotDanhGia = (SELECT COUNT(*) FROM DanhGiaMonAn WHERE MaMonAn = MonAn.MaMonAn),
        SoSaoTrungBinh = COALESCE((SELECT AVG(CAST(SoSao AS FLOAT)) FROM DanhGiaMonAn WHERE MaMonAn = MonAn.MaMonAn), 0)
    WHERE MaMonAn IN (
        SELECT MaMonAn FROM inserted
        UNION
        SELECT MaMonAn FROM deleted
    );

    -- Cập nhật cho bảng GianHang (DanhGiaTB là trung bình của TẤT CẢ các đánh giá của khách hàng cho các món trong gian hàng)
    UPDATE GianHang
    SET DanhGiaGianHang = COALESCE((
        SELECT AVG(CAST(d.SoSao AS FLOAT))
        FROM DanhGiaMonAn d
        INNER JOIN MonAn m2 ON d.MaMonAn = m2.MaMonAn
        WHERE m2.MaGianHang = GianHang.MaGianHang
    ), 0)
    WHERE MaGianHang IN (
        SELECT m.MaGianHang FROM MonAn m
        WHERE m.MaMonAn IN (
            SELECT MaMonAn FROM inserted
            UNION
            SELECT MaMonAn FROM deleted
        )
    );
END;
GO




/* =========================
   ĐIỀU LỆ (DieuLe)
========================= */
INSERT INTO DieuLe (TenDieuLe, PhiChietKhau, ChiTietDieuLe) VALUES
(N'Chính sách Tiêu chuẩn', 5.00, N'Nền tảng thu phí 5% trên mỗi đơn hàng giao dịch thành công. Đối tác tự chịu trách nhiệm chất lượng món ăn và vận chuyển.'),
(N'Chính sách Ưu đãi Mở mới', 3.00, N'Hỗ trợ quán mới mở: Thu phí 3% trên đơn hàng thành công trong 6 tháng đầu. Đối tác tự chịu trách nhiệm chất lượng món ăn và vận chuyển.');

/* =========================
   VAI TRÒ (VaiTro)
========================= */
INSERT INTO VaiTro (TenVaiTro) VALUES
(N'Admin'), (N'NhanVien'), (N'DoiTac'), (N'KhachHang');

/* =========================
   TÀI KHOẢN (TaiKhoan)
========================= */
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaVaiTro, TrangThai) VALUES
-- ADMIN
(N'quoc.nhk', '12345678', 1, N'Hoạt động'),       -- Quản trị cấp cao

-- NHÂN VIÊN HỆ THỐNG
(N'diep.ln', '12345678', 2, N'Hoạt động'),        -- Giao hàng
(N'vinh.tq', '12345678', 2, N'Hoạt động'),        -- Giao hàng 1
(N'tuan.ln', '12345678', 2, N'Hoạt động'),        -- Giao hàng 2
(N'thien.ph', '12345678', 2, N'Hoạt động'),       -- Giao hàng 3

-- ĐỐI TÁC
(N'highlands', '12345678', 3, N'Hoạt động'),
(N'phuclong', '12345678', 3, N'Hoạt động'),
(N'gongcha', '12345678', 3, N'Hoạt động'),
(N'comtamplt', '12345678', 3, N'Hoạt động'),
(N'baghien', '12345678', 3, N'Hoạt động'),
(N'phothin', '12345678', 3, N'Hoạt động'),
(N'phohoa', '12345678', 3, N'Hoạt động'),
(N'kfc', '12345678', 3, N'Hoạt động'),
(N'mcdonalds', '12345678', 3, N'Hoạt động'),
(N'haidilao', '12345678', 3, N'Hoạt động'),
(N'gogi', '12345678', 3, N'Hoạt động'),
(N'huynhhoa', '12345678', 3, N'Hoạt động'),
(N'sushitei', '12345678', 3, N'Hoạt động'),
(N'elgaucho', '12345678', 3, N'Hoạt động'),
(N'ranbien', '12345678', 3, N'Hoạt động'),
(N'chaybuddha', '12345678', 3, N'Hoạt động'),
(N'healthyfarm', '12345678', 3, N'Hoạt động'),

-- KHÁCH HÀNG
(N'baonam', '12345678', 4, N'Hoạt động'),
(N'thanhtruc', '12345678', 4, N'Hoạt động'),
(N'minhkhang', '12345678', 4, N'Hoạt động'),
(N'tramanh', '12345678', 4, N'Hoạt động'),
(N'quocviet', '12345678', 4, N'Hoạt động'),
(N'thaovy', '12345678', 4, N'Hoạt động'),
(N'tanphat', '12345678', 4, N'Hoạt động'),
(N'thuydieng', '12345678', 4, N'Hoạt động'),
(N'tuankiet', '12345678', 4, N'Hoạt động'),
(N'thuydung', '12345678', 4, N'Hoạt động');

/* =========================
   ĐỐI TÁC (DoiTac)
========================= */
INSERT INTO DoiTac (TenQuanDoiTac, SoDTDoiTac, DiaChiDoiTac, EmailDTac, TrangThaiDoiTac, MaTaiKhoan) VALUES
-- Cafe & Trà sữa
(N'Highlands Coffee', '08412345001', N'123 Lê Lợi', 'contact@highlandscoffee.vn', N'Còn đang hoạt động', 6),
(N'Phúc Long Coffee & Tea', '08412345002', N'45 Mạc Thị Bưởi', 'cskh@phuclong.com.vn', N'Còn đang hoạt động', 7),
(N'Gong Cha', '08412345006', N'79 Nguyễn Đình Chiểu', 'cskh@gongcha.com.vn', N'Còn đang hoạt động', 8),

-- Cơm tấm
(N'Cơm Tấm Phúc Lộc Thọ', '08412345011', N'236 Đinh Tiên Hoàng', 'plt@comtam.vn', N'Còn đang hoạt động', 9),
(N'Cơm Tấm Ba Ghiền', '08412345012', N'84 Đặng Văn Ngữ', 'baghien@gmail.com', N'Còn đang hoạt động', 10),

-- Phở & Bún
(N'Phở Thìn', '08412345021', N'13 Lò Đúc', 'phothin@gmail.com', N'Còn đang hoạt động', 11),
(N'Phở Hòa Pasteur', '08412345022', N'260C Pasteur', 'phohoa@gmail.com', N'Còn đang hoạt động', 12),

-- Fast Food
(N'KFC Việt Nam', '08412345041', N'292 Bà Triệu', 'contact@kfcvietnam.com.vn', N'Còn đang hoạt động', 13),
(N'McDonald''s Việt Nam', '08412345043', N'2-6 Bis Điện Biên Phủ', 'info@mcdonalds.vn', N'Còn đang hoạt động', 14),

-- Lẩu & Nướng
(N'Haidilao Hotpot', '08412345051', N'Bitexco Tower', 'cskh@haidilao.vn', N'Còn đang hoạt động', 15),
(N'Gogi House', '08412345054', N'128 Phan Xích Long', 'gogi@ggg.com.vn', N'Còn đang hoạt động', 16),

-- Bánh mì
(N'Bánh Mì Huynh Hoa', '08412345061', N'26 Lê Thị Riêng', 'huynhhoa@gmail.com', N'Còn đang hoạt động', 17),

-- Đồ Âu - Á
(N'Sushi Tei', '08412345074', N'200A Lý Tự Trọng', 'sushitei@gmail.com', N'Còn đang hoạt động', 18),
(N'El Gaucho Steakhouse', '08412345077', N'74/1 Hai Bà Trưng', 'elgaucho@gmail.com', N'Còn đang hoạt động', 19),

-- Hải sản & Healthy
(N'Hải Sản Rạn Biển', '08412345085', N'25 Kỳ Đồng', 'ranbien@gmail.com', N'Còn đang hoạt động', 20),
(N'Chay Buddha', '08412345092', N'31 Mạc Đĩnh Chi', 'buddha@gmail.com', N'Còn đang hoạt động', 21),
(N'Healthy Farm', '08412345096', N'12A Thảo Điền', 'healthyfarm@gmail.com', N'Còn đang hoạt động', 22);

/* =========================
   GIAN HÀNG (GianHang)
========================= */
INSERT INTO GianHang (MaDoiTac, MaDieuLe, TenGianHang, DiaChiCuThe, PhuongXa, ThanhPho, HinhAnh, GioMo, GioDong, DanhGiaGianHang, TrangThaiGianHang)
VALUES
-- Cafe & Trà sữa
(1, 1, N'Highlands Coffee - CN Lê Lợi', N'123 Lê Lợi', N'Phường Bến Nghé', N'TP. HCM', 'highlands.jpg', '07:00:00', '22:00:00', 4.8, N'Mở cửa'),

(2, 1, N'Phúc Long - CN Mạc Thị Bưởi', N'45 Mạc Thị Bưởi', N'Phường Bến Nghé', N'TP. HCM', 'phuclong.jpg', '07:00:00', '22:30:00', 4.9, N'Mở cửa'),

(3, 1, N'Gong Cha - Nguyễn Đình Chiểu', N'79 Nguyễn Đình Chiểu', N'Phường Bến Nghé', N'TP. HCM', 'gongcha.jpg', '08:00:00', '22:00:00', 4.7, N'Mở cửa'),


-- Cơm tấm
(4, 1, N'Cơm Tấm Phúc Lộc Thọ', N'236 Đinh Tiên Hoàng', N'Phường Bến Nghé', N'TP. HCM', 'comtam.jpg', '06:00:00', '21:00:00', 4.6, N'Mở cửa'),

(5, 1, N'Cơm Tấm Ba Ghiền', N'84 Đặng Văn Ngữ', N'Phường Bến Nghé', N'TP. HCM', 'baghien.jpg', '07:00:00', '20:00:00', 4.8, N'Mở cửa'),


-- Phở & Bún
(6, 1, N'Phở Thìn Lò Đúc', N'13 Lò Đúc', N'Phường Bến Nghé', N'Hà Nội', 'phothin.jpg', '06:00:00', '21:00:00', 4.8, N'Mở cửa'),

(7, 1, N'Phở Hòa Pasteur', N'260C Pasteur', N'Phường Bến Nghé', N'TP. HCM', 'phohoa.jpg', '06:00:00', '23:00:00', 4.7, N'Mở cửa'),


-- Fast Food
(8, 1, N'KFC - Bà Triệu', N'292 Bà Triệu', N'Phường Bến Nghé', N'Hà Nội', 'kfc.jpg', '09:00:00', '22:00:00', 4.6, N'Mở cửa'),

(9, 1, N'McDonald''s - Điện Biên Phủ', N'2-6 Bis Điện Biên Phủ', N'Phường Bến Nghé', N'TP. HCM', 'mcdonalds.jpg', '00:00:00', '23:59:00', 4.7, N'Mở cửa'),


-- Lẩu & Nướng
(10,1, N'Haidilao - Bitexco', N'Bitexco Tower', N'Phường Bến Nghé', N'TP. HCM', 'haidilao.jpg', '10:00:00', '23:00:00', 5.0, N'Mở cửa'),

(11,1, N'Gogi House', N'128 Phan Xích Long', N'Phường Bến Nghé', N'TP. HCM', 'gogi.jpg', '10:00:00', '22:00:00', 4.7, N'Mở cửa'),


-- Bánh mì
(12,1, N'Bánh Mì Huynh Hoa', N'26 Lê Thị Riêng', N'Phường Bến Nghé', N'TP. HCM', 'huynhhoa.jpg', '14:00:00', '23:00:00', 4.8, N'Mở cửa'),


-- Đồ Âu - Á
(13,1, N'Sushi Tei', N'200A Lý Tự Trọng', N'Phường Bến Nghé', N'TP. HCM', 'sushitei.jpg', '10:00:00', '22:30:00', 4.8, N'Mở cửa'),

(14,1, N'El Gaucho', N'74/1 Hai Bà Trưng', N'Phường Bến Nghé', N'TP. HCM', 'elgaucho.jpg', '11:00:00', '23:30:00', 4.9, N'Mở cửa'),


-- Hải sản & Chay
(15,1, N'Hải Sản Rạn Biển', N'25 Kỳ Đồng', N'Phường Bến Nghé', N'TP. HCM', 'ranbien.jpg', '10:00:00', '23:00:00', 4.8, N'Mở cửa'),


-- Nhóm Chay & Healthy
(16,1, N'Chay Buddha', N'31 Mạc Đĩnh Chi', N'Phường Bến Nghé', N'TP. HCM', 'chaybuddha.jpg', '09:00:00', '21:00:00', 4.9, N'Mở cửa'),

(17,1, N'Healthy Farm', N'12A Thảo Điền', N'Phường Bến Nghé', N'TP. HCM', 'healthyfarm.jpg', '07:00:00', '20:00:00', 4.7, N'Mở cửa')

/* =========================
   LOẠI MÓN ĂN
========================= */
INSERT INTO TheLoaiMonAn (TenLoai) VALUES
-- Nhóm 1: Bữa ăn chính
(N'Cơm'), (N'Bún / Phở / Mì'), (N'Bánh mì'), (N'Món chay'),
-- Nhóm 2: Ăn chơi & Giải khát
(N'Trà sữa'), (N'Đồ uống / Cà phê'), (N'Ăn vặt'), (N'Tráng miệng / Bánh ngọt'),
-- Nhóm 3: Đồ Âu & Fast Food
(N'Đồ ăn nhanh (Fast Food)'), (N'Gà rán'), (N'Pizza / Pasta'), (N'Bít tết / Món Âu'),
-- Nhóm 4: Món Á & Quốc tế
( N'Món Hàn Quốc'), (N'Món Nhật Bản / Sushi'), (N'Món Thái Lan'), (N'Món Hoa / Dimsum'),
-- Nhóm 5: Tụ tập & Đặc thù
(N'Lẩu & Nướng'), (N'Hải sản'), (N'Món Khỏe (Healthy / Salad)'), (N'Đặc sản vùng miền');

/* =========================
   MÓN ĂN (MonAn)
========================= */
INSERT INTO MonAn (MaGianHang, MaLoaiMon, TenMon, ThanhPhan, SoSaoTrungBinh, SoLuotDanhGia) VALUES
-- 1. Highlands Coffee (MaGianHang 1)
(1, 5, N'Trà sữa trân châu đường đen', N'Trà đen, sữa tươi, trân châu, đường đen', 0, 0),
 (1, 6, N'Cà phê sữa đá pha phin', N'Cà phê rang mộc, sữa đặc, đá', 0, 0),
 (1, 6, N'Bạc xỉu cốt dừa', N'Cà phê, sữa tươi, nước cốt dừa', 0, 0),
 (1, 5, N'Trà hạt sen Highlands', N'Trà xanh, hạt sen, đường', 0, 0),

-- 2. Phúc Long (MaGianHang 2)
(2, 5, N'Trà sữa Thái xanh', N'Trà thái, bột sữa', 0, 0),
 (2, 6, N'Trà đào đá xay', N'Đào, trà đen', 0, 0),
 (2, 5, N'Trà thiết quan âm', N'Trà Oolong', 0, 0),
 (2, 6, N'Hồng trà sữa', N'Hồng trà, sữa', 0, 0),

-- 3. Gong Cha (MaGianHang 3)
(3, 5, N'Trà sữa Oolong nướng', N'Trà oolong, bột sữa', 0, 0),
 (3, 8, N'Bánh mousse trà xanh', N'Trà xanh, kem', 0, 0),
 (3, 5, N'Trà ALisan trái cây', N'Trà Alisan, trái cây tươi', 0, 0),
 (3, 5, N'Trà sữa khoai môn', N'Bột khoai môn, sữa', 0, 0),

-- 4. Cơm Tấm PLT (MaGianHang 4)
(4, 1, N'Cơm tấm sườn bì chả', N'Cơm tấm, sườn nướng, bì, chả', 0, 0),
 (4, 1, N'Cơm tấm gà nướng', N'Cơm tấm, đùi gà nướng', 0, 0),
 (4, 1, N'Cơm tấm ba rọi nướng', N'Cơm tấm, thịt ba rọi', 0, 0),
 (4, 7, N'Bánh ít trần', N'Bột nếp, nhân tôm thịt', 0, 0),

-- 5. Cơm Tấm Ba Ghiền (MaGianHang 5)
(5, 1, N'Cơm tấm sườn non kho tộ', N'Cơm tấm, sườn non', 0, 0),
 (5, 7, N'Phá lấu lòng bò', N'Lòng bò, nước cốt dừa', 0, 0),
 (5, 1, N'Cơm tấm sườn cây', N'Cơm tấm, sườn cây nướng', 0, 0),
 (5, 1, N'Cơm tấm chả cua', N'Cơm tấm, chả cua', 0, 0),

-- 6. Phở Thìn (MaGianHang 6)
(6, 2, N'Phở bò tái nạm', N'Bánh phở, thịt bò nạm, nước dùng', 0, 0),
 (6, 2, N'Phở gà đặc biệt', N'Bánh phở, thịt gà ta', 0, 0),
 (6, 7, N'Bún chả Hà Nội', N'Bún tươi, chả nướng', 0, 0),
 (6, 2, N'Phở tái lăn', N'Bánh phở, bò xào lăn', 0, 0),

-- 7. Phở Hòa (MaGianHang 7)
(7, 2, N'Bún bò Huế đặc biệt', N'Bún, thịt bò, giò heo', 0, 0),
 (7, 2, N'Hủ tiếu Nam Vang', N'Hủ tiếu, tôm, thịt băm', 0, 0),
 (7, 2, N'Mì quảng gà', N'Mì quảng, thịt gà', 0, 0),
 (7, 2, N'Phở chín gầu', N'Bánh phở, gầu bò', 0, 0),

-- 8. KFC (MaGianHang 8)
(8, 9, N'Burger bò phô mai', N'Bánh mì, thịt bò, phô mai', 0, 0),
 (8, 10, N'Gà rán giòn cay', N'Gà tẩm bột chiên', 0, 0),
 (8, 7, N'Khoai tây chiên muối', N'Khoai tây, muối', 0, 0),
 (8, 10, N'Gà quay giấy bạc', N'Gà quay, tiêu xanh', 0, 0),

-- 9. McDonald's (MaGianHang 9)
(9, 11, N'Pizza hải sản nhiệt đới', N'Bột bánh, tôm, mực, dứa', 0, 0),
 (9, 11, N'Pizza xúc xích Ý', N'Bột bánh, xúc xích', 0, 0),
 (9, 9, N'Burger heo', N'Bánh mì, thịt heo xông khói', 0, 0),
 (9, 7, N'Nuggets gà', N'Thịt gà xay chiên giòn', 0, 0),

-- 10. Haidilao (MaGianHang 10)
(10, 17, N'Lẩu bò Ba Toa', N'Nước lẩu, thịt bò, rau xanh', 0, 0),
 (10, 17, N'Lẩu nấm thiên nhiên', N'Nước lẩu, các loại nấm', 0, 0),
 (10, 16, N'Mì múa Haidilao', N'Mì tươi', 0, 0),
 (10, 17, N'Thịt bò thượng hạng', N'Bò Wagyu thái lát', 0, 0),

-- 11. Gogi House (MaGianHang 11)
(11, 17, N'Thịt nướng Hàn Quốc', N'Thịt heo, sốt Hàn Quốc', 0, 0),
 (11, 13, N'Cơm trộn Bibimbap', N'Cơm, trứng, rau, thịt', 0, 0),
 (11, 17, N'Canh Kim Chi', N'Kim chi, thịt heo, đậu hũ', 0, 0),
 (11, 17, N'Dẻ sườn bò Mỹ', N'Dẻ sườn bò, sốt', 0, 0),

-- 12. Bánh Mì Huynh Hoa (MaGianHang 12)
(12, 3, N'Bánh mì thịt nướng', N'Bánh mì, thịt nướng, đồ chua', 0, 0),
 (12, 3, N'Bánh mì xá xíu', N'Bánh mì, thịt xá xíu', 0, 0),
 (12, 3, N'Bánh mì chả lụa', N'Bánh mì, chả lụa', 0, 0),
 (12, 3, N'Bánh mì pate đặc biệt', N'Bánh mì, pate thủ công', 0, 0),

-- 13. Sushi Tei (MaGianHang 13)
(13, 14, N'Sushi cá hồi tươi', N'Cơm, cá hồi sống', 0, 0),
 (13, 14, N'Sashimi tổng hợp', N'Cá hồi, cá ngừ, bạch tuộc', 0, 0),
 (13, 14, N'Tempura tôm', N'Tôm chiên xù', 0, 0),
 (13, 14, N'Cơm cuộn lươn', N'Cơm, lươn nướng mỡ', 0, 0),

-- 14. El Gaucho (MaGianHang 14)
(14, 12, N'Beefsteak bò Mỹ sốt tiêu đen', N'Thịt bò Mỹ, sốt tiêu đen', 0, 0),
 (14, 12, N'Mì Ý sốt bò bằm', N'Mì Ý, sốt cà chua, thịt bò', 0, 0),
 (14, 12, N'Khoai tây nghiền', N'Khoai tây, bơ, sữa', 0, 0),
 (14, 12, N'Rượu vang đỏ', N'Rượu nho nguyên chất', 0, 0),

-- 15. Rạn Biển (MaGianHang 15)
(15, 18, N'Cua hấp nước dừa', N'Cua biển, nước dừa', 0, 0),
 (15, 18, N'Tôm hùm nướng bơ tỏi', N'Tôm hùm, bơ, tỏi', 0, 0),
 (15, 18, N'Lẩu hải sản chua cay', N'Nước lẩu, tôm, mực, ngao', 0, 0),
 (15, 18, N'Ốc hương cháy tỏi', N'Ốc hương, tỏi phi', 0, 0),

-- 16. Chay Buddha (MaGianHang 16)
(16, 4, N'Cơm chiên chay', N'Cơm, đậu hũ, chả chay', 0, 0),
 (16, 4, N'Đậu hũ tứ xuyên chay', N'Đậu hũ non, nấm, sốt cay', 0, 0),
 (16, 4, N'Lẩu nấm chay', N'Nước dùng rau củ, các loại nấm', 0, 0),
 (16, 4, N'Gỏi cuốn chay', N'Bánh tráng, đậu hũ, rau sống', 0, 0),

-- 17. Healthy Farm (MaGianHang 17)
(17, 19, N'Salad ức gà áp chảo', N'Xà lách, ức gà, cà chua bi', 0, 0),
 (17, 19, N'Nước ép thanh lọc', N'Cần tây, táo, thơm', 0, 0),
 (17, 19, N'Sữa hạnh nhân', N'Hạt hạnh nhân xay', 0, 0),
 (17, 19, N'Hạt granola khô', N'Yến mạch, hạt óc chó, mật ong', 0, 0),

(1, 19, N'Trà thảo mộc thanh nhiệt', N'Thảo mộc tự nhiên', 0, 0),
 (2, 19, N'Nước ép bưởi mật ong', N'Bưởi tươi, mật ong', 0, 0),
 (3, 19, N'Sinh tố xanh Detox', N'Cải xoăn, táo, gừng', 0, 0);



/* =========================
   BIẾN THỂ MÓN ĂN (BienTheMonAn)
========================= */
INSERT INTO BienTheMonAn (MaMonAn, MoTaMonAn, SoLuongMon, GiaBan, HinhAnhMonAn, TrangThaiMonAn, MonAnMuaThem, GhiChu) VALUES
-- Biến thể cho MonAn 1-68 (mỗi MonAn 1 biến thể)
(1,  N'Size L, đường đen nhiều', 200, 35000, 'ts_duong_den.jpg', N'Còn bán', N'Trân châu trắng', N'Ít đá'),
(2,  N'Pha phin Đắk Lắk size M', 150, 25000, 'cf_sua.jpg', N'Còn bán', NULL, N'Ít đá'),
(3,  N'Bạc xỉu thơm béo size M', 100, 29000, 'bac_xiu.jpg', N'Còn bán', NULL, NULL),
(4,  N'Trà sen thơm mát size L', 120, 45000, 'tra_sen.jpg', N'Còn bán', NULL, NULL),
(5,  N'Size L kèm kem cheese', 180, 32000, 'ts_thai_xanh.jpg', N'Còn bán', N'Thạch trái cây', NULL),
(6,  N'Đào tươi miếng, đá xay mát lạnh', 90, 49000, 'tra_dao.jpg', N'Còn bán', NULL, NULL),
(7,  N'Trà Thiết Quan Âm đậm vị', 110, 39000, 'tra_tqa.jpg', N'Còn bán', NULL, NULL),
(8,  N'Hồng trà sữa truyền thống', 150, 35000, 'hong_tra.jpg', N'Còn bán', NULL, NULL),
(9,  N'Size L - Oolong nướng đậm vị', 120, 40000, 'ts_oolong.jpg', N'Còn bán', N'Trân châu đen', NULL),
(10, N'Bánh Mousse mềm mịn, thơm trà', 50, 45000, 'mousse_matcha.jpg', N'Còn bán', NULL, NULL),
(11, N'Trà trái cây nhiệt đới', 100, 42000, 'tra_alisan.jpg', N'Còn bán', NULL, NULL),
(12, N'Trà sữa khoai môn bùi béo', 80, 38000, 'ts_khoai_mon.jpg', N'Còn bán', NULL, NULL),
(13, N'Sườn to + chả + bì + mỡ hành', 100, 45000, 'com_tam.jpg', N'Còn bán', N'Canh khổ qua', NULL),
(14, N'Đùi gà nướng mật ong', 70, 48000, 'com_ga.jpg', N'Còn bán', NULL, NULL),
(15, N'Ba rọi nướng cháy cạnh', 85, 52000, 'com_ba_roi.jpg', N'Còn bán', NULL, NULL),
(16, N'Bánh ít trần nhân tôm thịt', 50, 30000, 'banh_it.jpg', N'Còn bán', NULL, NULL),
(17, N'Sườn non kho + dưa leo', 80, 48000, 'com_suon.jpg', N'Còn bán', NULL, NULL),
(18, N'Phá lấu nóng hổi kèm bánh mì', 60, 35000, 'pha_lau.jpg', N'Còn bán', NULL, NULL),
(19, N'Sườn cây nướng muối ớt', 40, 65000, 'com_suon_cay.jpg', N'Còn bán', NULL, NULL),
(20, N'Chả cua béo ngậy đặc biệt', 30, 45000, 'cha_cua.jpg', N'Còn bán', NULL, NULL),
(21, N'Tô lớn, bò Mỹ tái nạm', 120, 55000, 'pho_bo.jpg', N'Còn bán', N'Quẩy', N'Không hành'),
(22, N'Gà ta thả vườn, nước dùng thanh', 80, 50000, 'pho_ga.jpg', N'Còn bán', NULL, NULL),
(23, N'Bún chả nướng than hoa', 100, 45000, 'bun_cha.jpg', N'Còn bán', NULL, NULL),
(24, N'Phở bò tái lăn gừng tỏi', 90, 60000, 'pho_tai_lan.jpg', N'Còn bán', NULL, NULL),
(25, N'Đặc biệt đầy đủ topping', 90, 65000, 'bun_bo_hue.jpg', N'Còn bán', N'Chả cây', NULL),
(26, N'Hủ tiếu tôm thịt, mực tươi', 75, 55000, 'hu_tieu.jpg', N'Còn bán', NULL, NULL),
(27, N'Mì quảng gà ta đúng điệu', 60, 45000, 'mi_quang.jpg', N'Còn bán', NULL, NULL),
(28, N'Phở chín gầu giòn rụm', 70, 50000, 'pho_chin_gau.jpg', N'Còn bán', NULL, NULL),
(29, N'Burger bò nướng phô mai', 80, 55000, 'burger_bo.jpg', N'Còn bán', N'Phô mai lát', NULL),
(30, N'Gà rán giòn cay 1 miếng', 200, 35000, 'ga_cay.jpg', N'Còn bán', NULL, N'Nhiều tương ớt'),
(31, N'Khoai tây chiên vị muối tiêu', 150, 25000, 'khoai_tay.jpg', N'Còn bán', NULL, NULL),
(32, N'Gà quay giấy bạc thơm nức', 40, 125000, 'ga_quay.jpg', N'Còn bán', NULL, NULL),
(33, N'Pizza hải sản size M (20cm)', 40, 155000, 'pizza_hs.jpg', N'Còn bán', N'Viền phô mai', NULL),
(34, N'Pizza xúc xích Ý cao cấp', 35, 145000, 'pizza_pepperoni.jpg', N'Còn bán', NULL, NULL),
(35, N'Burger heo xông khói', 60, 45000, 'burger_heo.jpg', N'Còn bán', NULL, NULL),
(36, N'Nuggets gà 6 miếng', 100, 39000, 'nuggets.jpg', N'Còn bán', NULL, NULL),
(37, N'Lẩu bò cho 2 người', 25, 250000, 'lau_bo.jpg', N'Còn bán', N'Mì trứng', N'Nồi đất'),
(38, N'Lẩu nấm thập cẩm bồi bổ', 20, 220000, 'lau_nam.jpg', N'Còn bán', NULL, NULL),
(39, N'Mì kéo tươi múa tại bàn', 100, 50000, 'mi_mua.jpg', N'Còn bán', NULL, NULL),
(40, N'Thịt bò Wagyu thượng hạng', 15, 450000, 'bo_wagyu.jpg', N'Còn bán', NULL, NULL),
(41, N'Thịt nướng Hàn Quốc + kim chi', 60, 180000, 'thit_nuong_han.jpg', N'Còn bán', NULL, NULL),
(42, N'Cơm trộn truyền thống thố đá', 45, 85000, 'bibimbap.jpg', N'Còn bán', NULL, NULL),
(43, N'Canh kim chi đậu hũ nóng', 50, 45000, 'canh_kimchi.jpg', N'Còn bán', NULL, NULL),
(44, N'Dẻ sườn bò nướng sốt Gogi', 30, 195000, 'de_suon.jpg', N'Còn bán', NULL, NULL),
(45, N'Bánh mì thịt nướng + đồ chua', 150, 25000, 'bm_thit_nuong.jpg', N'Còn bán', NULL, N'Cay nhiều'),
(46, N'Bánh mì xá xíu Sài Gòn', 120, 25000, 'bm_xa_xiu.jpg', N'Còn bán', NULL, NULL),
(47, N'Bánh mì chả lụa truyền thống', 130, 22000, 'bm_cha_lua.jpg', N'Còn bán', NULL, NULL),
(48, N'Bánh mì đặc biệt đầy đủ pate', 100, 35000, 'bm_dac_biet.jpg', N'Còn bán', NULL, NULL),
(49, N'Sushi cá hồi tươi sống', 100, 95000, 'sushi_ca_hoi.jpg', N'Còn bán', N'Wasabi', NULL),
(69, N'Chai 330ml mát lạnh', 100, 35000, 'tra_thao_moc.jpg', N'Còn bán', NULL, NULL),
(70, N'Cốc 500ml nguyên chất', 80, 45000, 'nuoc_ep_buoi.jpg', N'Còn bán', NULL, NULL),
(71, N'Cốc 500ml detox', 90, 55000, 'detox.jpg', N'Còn bán', NULL, NULL),
(50, N'Set sashimi 5 loại cá tươi', 40, 250000, 'sashimi_set.jpg', N'Còn bán', NULL, NULL),
(51, N'Tempura tôm giòn tan', 60, 120000, 'tempura.jpg', N'Còn bán', NULL, NULL),
(52, N'Cơm cuộn lươn Nhật nướng', 35, 185000, 'unagi_roll.jpg', N'Còn bán', NULL, NULL),
(53, N'Beefsteak 200g - Medium Rare', 30, 185000, 'steak_tieu.jpg', N'Còn bán', N'Trứng ốp la', NULL),
(54, N'Mì Ý bò bằm sốt cà chua', 70, 85000, 'spaghetti.jpg', N'Còn bán', NULL, NULL),
(55, N'Khoai tây nghiền bơ sữa', 50, 35000, 'mashed_potato.jpg', N'Còn bán', NULL, NULL),
(56, N'Rượu vang đỏ El Gaucho', 20, 850000, 'wine_red.jpg', N'Còn bán', NULL, NULL),
(57, N'Cua Cà Mau hấp nước dừa', 15, 350000, 'cua_hap.jpg', N'Còn bán', N'Muối tiêu chanh', NULL),
(58, N'Tôm hùm xanh nướng bơ tỏi', 10, 550000, 'lobster.jpg', N'Còn bán', NULL, NULL),
(59, N'Lẩu hải sản chua cay Thái', 20, 280000, 'lau_hs.jpg', N'Còn bán', NULL, NULL),
(60, N'Ốc hương cháy tỏi thơm nức', 45, 150000, 'oc_huong.jpg', N'Còn bán', NULL, NULL),
(61, N'Cơm chiên chay đậu hạt', 60, 32000, 'com_chay.jpg', N'Còn bán', NULL, NULL),
(62, N'Đậu hũ sốt cay Tứ Xuyên chay', 50, 45000, 'tofu_spicy.jpg', N'Còn bán', NULL, NULL),
(63, N'Lẩu nấm tươi thanh đạm', 25, 185000, 'lau_nam_chay.jpg', N'Còn bán', NULL, NULL),
(64, N'Gỏi cuốn thực dưỡng chay', 80, 40000, 'goi_cuon_chay.jpg', N'Còn bán', NULL, NULL),
(65, N'Salad ức gà áp chảo tươi', 80, 65000, 'salad_ga.jpg', N'Còn bán', N'Sốt mè rang', N'Healthy'),
(66, N'Nước ép thanh lọc cơ thể', 100, 35000, 'detox.jpg', N'Còn bán', NULL, NULL),
(67, N'Sữa hạnh nhân nguyên chất', 50, 45000, 'almond_milk.jpg', N'Còn bán', NULL, NULL),
(68, N'Hạt Granola sấy khô mật ong', 40, 95000, 'granola.jpg', N'Còn bán', NULL, NULL);



/* =========================
   Khách hàng
========================= */
INSERT INTO KhachHang (TenKH, GioiTinhKH, SoDTKH, DiaChiCuThe, ThanhPho, DiemTichLuy, EmailKH, MaTaiKhoan) VALUES
(N'Nguyễn Bảo Nam', N'Nam', '01681000001', N'12 Lê Lợi', N'TP. HCM', 120, 'baonam.nguyen@gmail.com', 23),
(N'Trần Thị Thanh Trúc', N'Nữ', '01681000002', N'34 Hai Bà Trưng', N'TP. HCM', 450, 'thanhtruc.tran@gmail.com', 24),
(N'Lê Minh Khang', N'Nam', '01681000003', N'56 Nguyễn Đình Chiểu', N'Hà Nội', 1250, 'khang.leminh@gmail.com', 25),
(N'Phạm Ngọc Trâm Anh', N'Nữ', '01681000004', N'78 Nguyễn Thị Minh Khai', N'TP. HCM', 3400, 'tramanh.pham@gmail.com', 26),
(N'Hoàng Quốc Việt', N'Nam', '01681000005', N'90 Điện Biên Phủ', N'Đà Nẵng', 5600, 'quocviet.hoang@gmail.com', 27),
(N'Đinh Thảo Vy', N'Nữ', '01681000006', N'123 Cách Mạng Tháng 8', N'TP. HCM', 50, 'thaovy.dinh@gmail.com', 28),
(N'Bùi Tấn Phát', N'Nam', '01681000007', N'456 Lý Thường Kiệt', N'TP. HCM', 800, 'tanphat.bui@gmail.com', 29),
(N'Vũ Ngọc Thúy Diêng', N'Nữ', '01681000008', N'789 Lạc Long Quân', N'Hà Nội', 1500, 'thuydieng.vu@gmail.com', 30),
(N'Đặng Tuấn Kiệt', N'Nam', '01681000009', N'12A Phan Xích Long', N'TP. HCM', 4100, 'tuankiet.dang@gmail.com', 31),
(N'Ngô Thị Thùy Dung', N'Nữ', '01681000010', N'34B Quang Trung', N'Hà Nội', 6200, 'thuydung.ngo@gmail.com', 32);

/* =========================
   DỮ LIỆU MẪU ĐỊA CHỈ
========================= */
INSERT INTO DiaChi (MaKh, MaGianHang, TenGoiNho, TenNguoiNhan, SoDienThoaiNhan, DiaChiCuThe, PhuongXa, ThanhPho, ViDo, KinhDo, LaMacDinh, DaXoa) VALUES
(1, NULL, N'Nhà riêng', N'Nguyễn Bảo Nam', '01681000001', N'12 Lê Lợi', N'Bến Thành', N'TP. HCM', 10.7769, 106.7009, 1, 0),
(2, NULL, N'Công ty', N'Trần Thị Mai', '0912345678', N'45 Nguyễn Huệ', N'Bến Nghé', N'TP. HCM', 10.7743, 106.7042, 1, 0),
(3, NULL, N'Nhà riêng', N'Lê Văn Tâm', '0987654321', N'89 Phạm Văn Đồng', N'Hiệp Bình Chánh', N'Thủ Đức', 10.8286, 106.7214, 1, 0),
(4, NULL, N'Nhà trọ', N'Phạm Thu Thủy', '0901112233', N'221 Điện Biên Phủ', N'Phường 15', N'Bình Thạnh', 10.8005, 106.7058, 1, 0),
(5, NULL, N'Cửa hàng', N'Hoàng Tuấn Anh', '0934567890', N'15 Nguyễn Trãi', N'Phường 2', N'Quận 5', 10.7602, 106.6800, 1, 0),
(6, NULL, N'Ký túc xá', N'Bùi Bích Hữu', '0966778899', N'Khu phố 6', N'Linh Trung', N'Thủ Đức', 10.8700, 106.8030, 1, 0),
(7, NULL, N'Nhà riêng', N'Ngô Đình Khôi', '0971234567', N'120 Hai Bà Trưng', N'Tân Định', N'Quận 1', 10.7871, 106.6963, 1, 0),
(8, NULL, N'Văn phòng', N'Võ Hoàng Yến', '0922334455', N'200 Võ Văn Tần', N'Phường 5', N'Quận 3', 10.7749, 106.6853, 1, 0),
(9, NULL, N'Nhà riêng', N'Đặng Lê Nguyên', '0945678123', N'75 Lý Thường Kiệt', N'Phường 15', N'Quận 11', 10.7692, 106.6570, 1, 0),
(10, NULL, N'Chung cư', N'Trịnh Công Sơn', '0999888777', N'Chung cư The Manor', N'Phường 22', N'Bình Thạnh', 10.7955, 106.7176, 1, 0);


/* =============================================
    PHƯƠNG THỨC THANH TOÁN
============================================= */
INSERT INTO PhuongThucThToan (TenPhuongThuc) VALUES
(N'Tiền mặt'),
(N'Ví điện tử'),
(N'Thẻ');

-- ====================================================================
-- GIỎ HÀNG
-- ====================================================================
INSERT INTO GioHang (MaKH) VALUES
(1),(2),(3),(4),(5),(6),(7),(8),(9),(10);

-- ====================================================================
-- CHI TIẾT GIỎ HÀNG
-- ====================================================================
INSERT INTO CT_GioHang (MaGioHang, MaBienThe, SoLuong, GiaBan, UocTinhThanhTien) VALUES
(1, 1, 2, 35000, 70000),
(1, 2, 1, 25000, 25000),
(2, 5, 1, 45000, 45000),
(3, 7, 1, 55000, 55000),
(4, 9, 2, 55000, 110000),
(5, 10, 1, 35000, 35000),
(6, 12, 1, 250000, 250000),
(7, 14, 2, 25000, 50000),
(8, 15, 1, 95000, 95000),
(9, 16, 1, 185000, 185000),
(10, 17, 1, 350000, 350000);

/* =============================================
   ĐƠN HÀNG (DonHang)
============================================= */
INSERT INTO DonHang
    (MaKH, MaGianHang, NgayTaoDon, TongTienMon, GiamGia, ThanhTienKhachTra, ThanhTienQuanNhan, TrangThaiDonHang)
VALUES
(1, 1, '2026-04-01 10:30:00', 70000, 0, 70000, 70000, N'Hoàn thành'),
(2, 2, '2026-04-02 12:15:00', 45000, 0, 45000, 45000, N'Hoàn thành'),
(3, 4, '2026-04-03 18:45:00', 45000, 0, 45000, 45000, N'Hoàn thành'),
(4, 6, '2026-04-04 19:30:00', 55000, 0, 55000, 55000, N'Hoàn thành'),
(5, 8, '2026-04-05 20:00:00', 110000,10000,100000, 100000, N'Hoàn thành'),
(6,10,'2026-04-06 11:20:00', 250000, 0, 250000, 250000, N'Hoàn thành'),
(7,12,'2026-04-07 17:50:00', 50000, 0, 50000, 50000, N'Hoàn thành'),
(8,13,'2026-04-08 19:10:00', 95000, 0, 95000, 95000, N'Hoàn thành'),
(9,14,'2026-04-09 20:30:00', 185000,15000,170000, 170000, N'Hoàn thành'),
(10,15,'2026-04-10 21:15:00',350000, 0, 350000, 350000, N'Hoàn thành'),
(1, 1, '2026-04-11 09:45:00', 35000, 0, 35000, 35000, N'Đang giao'),
(2, 4, '2026-04-12 12:30:00', 90000, 0, 90000, 90000, N'Đang chuẩn bị'),
(3, 6, '2026-04-13 18:20:00',110000, 0,110000, 110000, N'Chờ xác nhận'),
(4,10,'2026-04-14 19:40:00',250000,20000,230000, 230000, N'Hoàn thành'),
(5,13,'2026-04-15 20:55:00', 95000, 0, 95000, 95000, N'Hoàn thành'),
(6, 1, '2026-04-16 10:00:00', 50000, 0, 50000, 50000, N'Đã hủy');

UPDATE DonHang SET LyDoHuy = N'Đợi quá lâu' WHERE MaDonHang = 16;

/* =============================================
   CHI TIẾT ĐƠN HÀNG (CT_DonHang)
============================================= */
INSERT INTO CT_DonHang (MaDonHang, MaBienThe, SoLuongMua, GiaBan) VALUES
(1, 1, 2, 35000),
(2, 5, 1, 45000),
(3, 7, 1, 55000),
(4, 9, 2, 55000),
(5,10, 1, 35000),
(6,12, 1,250000),
(7,14, 2, 25000),
(8,15, 1, 95000),
(9,16, 1,185000),
(10,17,1,350000),
(11, 2,1, 25000),
(12, 5,2, 45000),
(13, 7,2, 55000),
(14,12,1,250000),
(15,15,1, 95000),
(16, 1, 1, 50000);

/* =============================================
   Thanh Toán (ThanhToan)
============================================= */
INSERT INTO ThanhToan (MaDonHang, MaPTThanhToan, SoTienThanhToan, TrangThaiThanhToan, GhiChuThanhToan) VALUES
(1, 1, 70000, N'Đã thanh toán', N'Tiền mặt'),
(2, 2, 45000, N'Đã thanh toán', N'Ví MoMo'),
(3, 1, 45000, N'Đã thanh toán', N'Tiền mặt'),
(4, 3, 55000, N'Đã thanh toán', N'Thẻ Visa'),
(5, 2,100000, N'Đã thanh toán', N'Ví điện tử'),
(6, 1,250000, N'Đã thanh toán', N'Tiền mặt'),
(7, 2, 50000, N'Đã thanh toán', N'Ví ZaloPay'),
(8, 3, 95000, N'Đã thanh toán', N'Thẻ'),
(9, 1,170000, N'Đã thanh toán', N'Tiền mặt'),
(10,2,350000, N'Đã thanh toán', N'Ví điện tử'),
(11,1, 35000, N'Đã thanh toán', N'Tiền mặt'),
(12,2, 90000, N'Đã thanh toán', N'Ví MoMo'),
(13,3,110000, N'Đã thanh toán', N'Thẻ'),
(14,1,230000, N'Đã thanh toán', N'Tiền mặt'),
(15,2, 95000, N'Đã thanh toán', N'Ví điện tử');

/* =============================================
   GIAO HÀNG (GiaoHang)
============================================= */
INSERT INTO GiaoHang
    (MaNV, MaDonHang, PhiVanChuyen, TGianXuatPhat, TGianDuKienDen, TrangThaiGHang)
VALUES
(4, 1, 15000, '10:45', '11:30', N'Đã hoàn thành'),
(5, 2, 15000, '12:30', '13:15', N'Đã hoàn thành'),
(4, 3, 20000, '19:00', '19:50', N'Đã hoàn thành'),
(5, 4, 15000, '19:45', '20:30', N'Đã hoàn thành'),
(3, 5, 25000, '20:15', '21:10', N'Đã hoàn thành'),
(4, 6, 30000, '11:30', '12:40', N'Đã hoàn thành'),
(5, 7, 15000, '18:00', '18:45', N'Đã hoàn thành'),
(3, 8, 20000, '19:20', '20:10', N'Đã hoàn thành'),
(4, 9, 25000, '20:40', '21:35', N'Đã hoàn thành'),
(5,10, 30000, '21:20', '22:30', N'Đã hoàn thành'),
(3,11, 15000, '09:50', '10:40', N'Đang giao'),
(4,12, 15000, '12:40', '13:30', N'Tài xế sắp đến'),
(5,13, 20000, '18:20', '19:10', N'Đang lấy đơn');

/* =========================
   DỮ LIỆU MẪU ĐÁNH GIÁ MÓN ĂN
========================= */
INSERT INTO DanhGiaMonAn (MaMonAn, MaKH, SoSao, NoiDung, NgayDanhGia) VALUES
(1, 1, 5, N'Trà sữa rất ngon, trân châu dai giòn đúng ý!', GETDATE()),
(1, 2, 4, N'Hơi ngọt một chút nhưng vẫn rất ổn.', GETDATE()),
(2, 3, 5, N'Cà phê đậm đà, tỉnh táo cả ngày.', GETDATE()),
(5, 4, 5, N'Trà thái xanh thơm, kem cheese béo ngậy.', GETDATE()),
(13, 5, 5, N'Cơm tấm sườn bì chả đỉnh nhất Sài Gòn!', GETDATE()),
(14, 6, 4, N'Gà nướng ngon nhưng hơi nhỏ.', GETDATE()),
(21, 7, 5, N'Nước phở ngon, đậm vị Bắc.', GETDATE()),
(21, 8, 5, N'Giá hợp lý, chất lượng tuyệt vời.', GETDATE());

/* =========================
   LỊCH SỬ TÌM KIẾM
========================= */



/* =========================
   TRIGGER TỰ ĐỘNG CẬP NHẬT SỐ LƯỢT BÁN (SoLuotBan)
========================= */
GO
CREATE TRIGGER trg_UpdateSoLuotBan
ON DonHang
AFTER UPDATE
AS
BEGIN
    IF UPDATE(TrangThaiDonHang)
    BEGIN
        -- Tăng SoLuotBan khi đơn hàng chuyển sang 'Hoàn thành'
        UPDATE m
        SET m.SoLuotBan = ISNULL(m.SoLuotBan, 0) + c.SoLuongMua
        FROM MonAn m
        JOIN BienTheMonAn b ON m.MaMonAn = b.MaMonAn
        JOIN CT_DonHang c ON b.MaBienThe = c.MaBienThe
        JOIN inserted i ON c.MaDonHang = i.MaDonHang
        JOIN deleted d ON i.MaDonHang = d.MaDonHang
        WHERE i.TrangThaiDonHang = N'Hoàn thành' AND d.TrangThaiDonHang <> N'Hoàn thành';

        -- Giảm SoLuotBan nếu đơn hàng từ 'Hoàn thành' chuyển sang trạng thái khác (hủy)
        UPDATE m
        SET m.SoLuotBan = CASE WHEN ISNULL(m.SoLuotBan, 0) - c.SoLuongMua < 0 THEN 0 ELSE ISNULL(m.SoLuotBan, 0) - c.SoLuongMua END
        FROM MonAn m
        JOIN BienTheMonAn b ON m.MaMonAn = b.MaMonAn
        JOIN CT_DonHang c ON b.MaBienThe = c.MaBienThe
        JOIN inserted i ON c.MaDonHang = i.MaDonHang
        JOIN deleted d ON i.MaDonHang = d.MaDonHang
        WHERE d.TrangThaiDonHang = N'Hoàn thành' AND i.TrangThaiDonHang <> N'Hoàn thành';
    END
END
GO

/* =========================
   DỮ LIỆU DOANH THU MẪU (7 NGÀY GẦN NHẤT) CHO BIỂU ĐỒ
========================= */
DECLARE @today DATETIME = GETDATE();
DECLARE @i INT = 0;
DECLARE @newOrderId INT;

WHILE @i < 7
BEGIN
    DECLARE @orderDate DATETIME = DATEADD(day, -@i, @today);

    -- Gian Hàng 1
    INSERT INTO DonHang (MaKH, MaGianHang, NgayTaoDon, TongTienMon, GiamGia, ThanhTienKhachTra, ThanhTienQuanNhan, TrangThaiDonHang)
    VALUES (1, 1, @orderDate, 150000 + (@i * 10000), 0, 150000 + (@i * 10000), 150000 + (@i * 10000), N'Hoàn thành');
    SET @newOrderId = SCOPE_IDENTITY();
    INSERT INTO CT_DonHang (MaDonHang, MaBienThe, SoLuongMua, GiaBan) VALUES (@newOrderId, 1, 1, 150000 + (@i * 10000));
    
    -- Gian Hàng 2
    INSERT INTO DonHang (MaKH, MaGianHang, NgayTaoDon, TongTienMon, GiamGia, ThanhTienKhachTra, ThanhTienQuanNhan, TrangThaiDonHang)
    VALUES (2, 2, @orderDate, 200000 - (@i * 5000), 0, 200000 - (@i * 5000), 200000 - (@i * 5000), N'Hoàn thành');
    SET @newOrderId = SCOPE_IDENTITY();
    INSERT INTO CT_DonHang (MaDonHang, MaBienThe, SoLuongMua, GiaBan) VALUES (@newOrderId, 5, 1, 200000 - (@i * 5000));
    
    -- Gian Hàng 4
    INSERT INTO DonHang (MaKH, MaGianHang, NgayTaoDon, TongTienMon, GiamGia, ThanhTienKhachTra, ThanhTienQuanNhan, TrangThaiDonHang)
    VALUES (3, 4, @orderDate, 80000 + (@i * 15000), 0, 80000 + (@i * 15000), 80000 + (@i * 15000), N'Hoàn thành');
    SET @newOrderId = SCOPE_IDENTITY();
    INSERT INTO CT_DonHang (MaDonHang, MaBienThe, SoLuongMua, GiaBan) VALUES (@newOrderId, 7, 1, 80000 + (@i * 15000));
    
    SET @i = @i + 1;
END

/* =========================
   SỬA LỖI DỮ LIỆU: CẬP NHẬT TRẠNG THÁI THANH TOÁN CHO ĐƠN HOÀN THÀNH
   (Đảm bảo các đơn đã hoàn thành đều được tính vào doanh thu)
========================= */
UPDATE DonHang
SET TrangThaiThanhToan = N'Đã thanh toán'
WHERE TrangThaiDonHang = N'Hoàn thành' AND TrangThaiThanhToan = N'Chưa thanh toán';

/* =========================
   SỬA LỖI ĐÁNH GIÁ (ĐỒNG BỘ THEO DỮ LIỆU THỰC TẾ)
========================= */
-- 1. Reset toàn bộ số liệu về 0 (tránh trường hợp chưa có đánh giá nào nhưng vẫn hiện sao ảo)
UPDATE MonAn SET SoSaoTrungBinh = 0, SoLuotDanhGia = 0;
UPDATE GianHang SET DanhGiaGianHang = '0';

-- 2. Cập nhật lại Món Ăn từ dữ liệu thật trong bảng DanhGiaMonAn
UPDATE m
SET
    m.SoLuotDanhGia = (SELECT COUNT(*) FROM DanhGiaMonAn WHERE MaMonAn = m.MaMonAn),
    m.SoSaoTrungBinh = COALESCE((SELECT AVG(CAST(SoSao AS FLOAT)) FROM DanhGiaMonAn WHERE MaMonAn = m.MaMonAn), 0)
FROM MonAn m;

-- 3. Cập nhật lại Gian Hàng từ dữ liệu thật trong bảng DanhGiaMonAn
UPDATE g
SET DanhGiaGianHang = COALESCE((
    SELECT AVG(CAST(d.SoSao AS FLOAT))
    FROM DanhGiaMonAn d
    INNER JOIN MonAn m2 ON d.MaMonAn = m2.MaMonAn
    WHERE m2.MaGianHang = g.MaGianHang
), 0)
FROM GianHang g;

/* =========================================================================
   BƯỚC ĐỒNG BỘ ĐÁNH GIÁ: Cập nhật lại số liệu cho MonAn và GianHang
========================================================================= */
UPDATE MonAn
SET SoLuotDanhGia = (SELECT COUNT(*) FROM DanhGiaMonAn WHERE DanhGiaMonAn.MaMonAn = MonAn.MaMonAn),
    SoSaoTrungBinh = ISNULL((SELECT AVG(CAST(SoSao AS DECIMAL(3,1))) FROM DanhGiaMonAn WHERE DanhGiaMonAn.MaMonAn = MonAn.MaMonAn), 0);

UPDATE GianHang
SET DanhGiaGianHang = (
    SELECT CAST(CAST(ISNULL(AVG(SoSaoTrungBinh), 0) AS DECIMAL(3,1)) AS NVARCHAR(50)) 
    FROM MonAn 
    WHERE MonAn.MaGianHang = GianHang.MaGianHang AND MonAn.SoLuotDanhGia > 0
)
WHERE EXISTS (SELECT 1 FROM MonAn WHERE MonAn.MaGianHang = GianHang.MaGianHang AND MonAn.SoLuotDanhGia > 0);

/* =========================
   Chương Trình Khuyến Mãi (10 records gộp từ Khuyến Mãi)
========================= */
INSERT INTO ChuongTrinhKhuyenMai (TenChTrinh, NoiDungKMai, NgayBDau, NgayKThuc, PhanTramGiam, GiamToiDa, DieuKienApDung, SuDung1Lan, LoaiKhuyenMai, TrangThaiKMai, MaGianHang) VALUES
(N'Chào mừng thành viên mới', 'NEWBIE10', '2024-01-01', '2024-12-31', 10, 50000, 30000, 1, N'Giảm giá toàn hệ thống', N'Kích hoạt', NULL),
(N'Flash Sale cuối tuần', 'WEEKEND20', '2024-05-01', '2024-12-31', 20, 100000, 50000, 0, N'Giảm giá món ăn', N'Kích hoạt', NULL),
(N'Siêu ưu đãi tháng 5', 'MAY50', '2024-05-01', '2024-05-31', 50, 200000, 100000, 1, N'Sale khủng', N'Kích hoạt', NULL),
(N'Miễn phí ship đơn đầu', 'FREESHIP', '2024-01-01', '2024-12-31', 15, 30000, 0, 1, N'Hỗ trợ ship', N'Kích hoạt', NULL),
(N'Khuyến mãi Lễ 30/4', 'HOLIDAY30', '2024-04-25', '2024-05-05', 30, 80000, 80000, 1, N'Lễ hội', N'Đã hết thời gian sử dụng', NULL),
(N'Trà sữa đồng giá', 'TRASUA20K', '2024-06-01', '2024-07-01', 20, 40000, 50000, 0, N'Giảm giá thức uống', N'Kích hoạt', NULL),
(N'Combo Cơm Trưa Sale', 'COMSALE15', '2024-06-01', '2024-12-31', 15, 25000, 60000, 0, N'Giảm giá đồ ăn', N'Kích hoạt', NULL),
(N'Món mới giảm 30%', 'MONMOI30', '2024-06-10', '2024-06-20', 30, 150000, 100000, 1, N'Trải nghiệm', N'Kích hoạt', NULL),
(N'Ăn vặt cuối ngày', 'NIGHTSNACK', '2024-01-01', '2024-12-31', 10, 20000, 40000, 0, N'Ăn vặt', N'Kích hoạt', NULL),
(N'Khuyến mãi nhóm bạn', 'GROUP25', '2024-06-01', '2024-12-31', 25, 200000, 300000, 0, N'Khuyến mãi nhóm', N'Kích hoạt', NULL);


/* Cập nhật Địa Chỉ cho Đơn Hàng */
UPDATE DonHang SET MaDiaChi = MaKH WHERE MaDiaChi IS NULL;

/* =========================
   DỮ LIỆU 168 PHƯỜNG/XÃ MỚI SÁP NHẬP
========================= */
INSERT INTO DiaChi (MaKh, MaGianHang, TenGoiNho, TenNguoiNhan, SoDienThoaiNhan, DiaChiCuThe, PhuongXa, ThanhPho, ViDo, KinhDo, LaMacDinh, DaXoa) VALUES
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000000', N'Nhà số 1', N'Phường Sài Gòn', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000001', N'Nhà số 2', N'Phường Tân Định', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000002', N'Nhà số 3', N'Phường Bến Thành', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000003', N'Nhà số 4', N'Phường Cầu Ông Lãnh', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000004', N'Nhà số 5', N'Phường Bàn Cờ', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000005', N'Nhà số 6', N'Phường Xuân Hòa', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000006', N'Nhà số 7', N'Phường Nhiêu Lộc', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000007', N'Nhà số 8', N'Phường Xóm Chiếu', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000008', N'Nhà số 9', N'Phường Khánh Hội', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000009', N'Nhà số 10', N'Phường Vĩnh Hội', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000010', N'Nhà số 11', N'Phường Chợ Quán', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000011', N'Nhà số 12', N'Phường An Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000012', N'Nhà số 13', N'Phường Chợ Lớn', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000013', N'Nhà số 14', N'Phường Bình Tây', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000014', N'Nhà số 15', N'Phường Bình Tiên', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000015', N'Nhà số 16', N'Phường Bình Phú', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000016', N'Nhà số 17', N'Phường Phú Lâm', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000017', N'Nhà số 18', N'Phường Tân Thuận', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000018', N'Nhà số 19', N'Phường Phú Thuận', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000019', N'Nhà số 20', N'Phường Tân Mỹ', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000020', N'Nhà số 21', N'Phường Tân Hưng', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000021', N'Nhà số 22', N'Phường Chánh Hưng', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000022', N'Nhà số 23', N'Phường Phú Định', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000023', N'Nhà số 24', N'Phường Bình Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000024', N'Nhà số 25', N'Phường Diên Hồng', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000025', N'Nhà số 26', N'Phường Vườn Lài', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000026', N'Nhà số 27', N'Phường Hòa Hưng', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000027', N'Nhà số 28', N'Phường Minh Phụng', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000028', N'Nhà số 29', N'Phường Bình Thới', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000029', N'Nhà số 30', N'Phường Hòa Bình', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000030', N'Nhà số 31', N'Phường Phú Thọ', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000031', N'Nhà số 32', N'Phường Đông Hưng Thuận', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000032', N'Nhà số 33', N'Phường Trung Mỹ Tây', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000033', N'Nhà số 34', N'Phường Tân Thới Hiệp', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000034', N'Nhà số 35', N'Phường Thới An', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000035', N'Nhà số 36', N'Phường An Phú Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000036', N'Nhà số 37', N'Phường An Lạc', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000037', N'Nhà số 38', N'Phường Bình Tân', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000038', N'Nhà số 39', N'Phường Tân Tạo', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000039', N'Nhà số 40', N'Phường Bình Trị Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000040', N'Nhà số 41', N'Phường Bình Hưng Hòa', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000041', N'Nhà số 42', N'Phường Gia Định', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000042', N'Nhà số 43', N'Phường Bình Thạnh', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000043', N'Nhà số 44', N'Phường Bình Lợi Trung', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000044', N'Nhà số 45', N'Phường Thạnh Mỹ Tây', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000045', N'Nhà số 46', N'Phường Bình Quới', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000046', N'Nhà số 47', N'Phường Hạnh Thông', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000047', N'Nhà số 48', N'Phường An Nhơn', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000048', N'Nhà số 49', N'Phường Gò Vấp', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000049', N'Nhà số 50', N'Phường An Hội Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000050', N'Nhà số 51', N'Phường Thông Tây Hội', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000051', N'Nhà số 52', N'Phường An Hội Tây', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000052', N'Nhà số 53', N'Phường Đức Nhuận', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000053', N'Nhà số 54', N'Phường Cầu Kiệu', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000054', N'Nhà số 55', N'Phường Phú Nhuận', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000055', N'Nhà số 56', N'Phường Tân Sơn Hòa', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000056', N'Nhà số 57', N'Phường Tân Sơn Nhất', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000057', N'Nhà số 58', N'Phường Tân Hòa', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000058', N'Nhà số 59', N'Phường Bảy Hiền', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000059', N'Nhà số 60', N'Phường Tân Bình', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000060', N'Nhà số 61', N'Phường Tân Sơn', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000061', N'Nhà số 62', N'Phường Tây Thạnh', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000062', N'Nhà số 63', N'Phường Tân Sơn Nhì', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000063', N'Nhà số 64', N'Phường Phú Thọ Hòa', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000064', N'Nhà số 65', N'Phường Tân Phú', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000065', N'Nhà số 66', N'Phường Phú Thạnh', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000066', N'Nhà số 67', N'Phường Hiệp Bình', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000067', N'Nhà số 68', N'Phường Thủ Đức', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000068', N'Nhà số 69', N'Phường Tam Bình', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000069', N'Nhà số 70', N'Phường Linh Xuân', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000070', N'Nhà số 71', N'Phường Tăng Nhơn Phú', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000071', N'Nhà số 72', N'Phường Long Bình', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000072', N'Nhà số 73', N'Phường Long Phước', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000073', N'Nhà số 74', N'Phường Long Trường', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000074', N'Nhà số 75', N'Phường Cát Lái', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000075', N'Nhà số 76', N'Phường Bình Trưng', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000076', N'Nhà số 77', N'Phường Phước Long', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000077', N'Nhà số 78', N'Phường An Khánh', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000078', N'Nhà số 79', N'Phường Đông Hòa', N'Bình Dương', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000079', N'Nhà số 80', N'Phường Dĩ An', N'Bình Dương', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000080', N'Nhà số 81', N'Phường Tân Đông Hiệp', N'Bình Dương', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000081', N'Nhà số 82', N'Phường An Phú', N'Bình Dương', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000082', N'Nhà số 83', N'Phường Bình Hòa', N'Bình Dương', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000083', N'Nhà số 84', N'Phường Lái Thiêu', N'Bình Dương', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000084', N'Nhà số 85', N'Phường Thuận An', N'Bình Dương', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000085', N'Nhà số 86', N'Phường Thuận Giao', N'Bình Dương', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000086', N'Nhà số 87', N'Phường Thủ Dầu Một', N'Bình Dương', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000087', N'Nhà số 88', N'Phường Phú Lợi', N'Bình Dương', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000088', N'Nhà số 89', N'Phường Chánh Hiệp', N'Bình Dương', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000089', N'Nhà số 90', N'Phường Bình Dương', N'Bình Dương', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000090', N'Nhà số 91', N'Phường Hòa Lợi', N'Bình Dương', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000091', N'Nhà số 92', N'Phường Phú An', N'Bình Dương', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000092', N'Nhà số 93', N'Phường Tây Nam', N'Bình Dương', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000093', N'Nhà số 94', N'Phường Long Nguyên', N'Bình Dương', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000094', N'Nhà số 95', N'Phường Bến Cát', N'Bình Dương', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000095', N'Nhà số 96', N'Phường Chánh Phú Hòa', N'Bình Dương', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000096', N'Nhà số 97', N'Phường Vĩnh Tân', N'Bình Dương', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000097', N'Nhà số 98', N'Phường Bình Cơ', N'Bình Dương', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000098', N'Nhà số 99', N'Phường Tân Uyên', N'Bình Dương', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000099', N'Nhà số 100', N'Phường Tân Hiệp', N'Bình Dương', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000100', N'Nhà số 101', N'Phường Tân Khánh', N'Bình Dương', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000101', N'Nhà số 102', N'Phường Vũng Tàu', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000102', N'Nhà số 103', N'Phường Tam Thắng', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000103', N'Nhà số 104', N'Phường Rạch Dừa', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000104', N'Nhà số 105', N'Phường Phước Thắng', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000105', N'Nhà số 106', N'Phường Long Hương', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000106', N'Nhà số 107', N'Phường Bà Rịa', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000107', N'Nhà số 108', N'Phường Tam Long', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000108', N'Nhà số 109', N'Phường Tân Hải', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000109', N'Nhà số 110', N'Phường Tân Phước', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000110', N'Nhà số 111', N'Phường Phú Mỹ', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000111', N'Nhà số 112', N'Phường Tân Thành', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000112', N'Nhà số 113', N'Xã Vĩnh Lộc', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000113', N'Nhà số 114', N'Xã Tân Vĩnh Lộc', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000114', N'Nhà số 115', N'Xã Bình Lợi', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000115', N'Nhà số 116', N'Xã Tân Nhựt', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000116', N'Nhà số 117', N'Xã Bình Chánh', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000117', N'Nhà số 118', N'Xã Hưng Long', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000118', N'Nhà số 119', N'Xã Bình Hưng', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000119', N'Nhà số 120', N'Xã Bình Khánh', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000120', N'Nhà số 121', N'Xã An Thới Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000121', N'Nhà số 122', N'Xã Cần Giờ', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000122', N'Nhà số 123', N'Xã Củ Chi', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000123', N'Nhà số 124', N'Xã Tân An Hội', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000124', N'Nhà số 125', N'Xã Thái Mỹ', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000125', N'Nhà số 126', N'Xã An Nhơn Tây', N'TP.HCM', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000126', N'Nhà số 127', N'Xã Nhuận Đức', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000127', N'Nhà số 128', N'Xã Phú Hòa Đông', N'TP.HCM', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000128', N'Nhà số 129', N'Xã Bình Mỹ', N'TP.HCM', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000129', N'Nhà số 130', N'Xã Đông Thạnh', N'TP.HCM', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000130', N'Nhà số 131', N'Xã Hóc Môn', N'TP.HCM', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000131', N'Nhà số 132', N'Xã Xuân Thới Sơn', N'TP.HCM', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000132', N'Nhà số 133', N'Xã Bà Điểm', N'TP.HCM', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000133', N'Nhà số 134', N'Xã Nhà Bè', N'TP.HCM', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000134', N'Nhà số 135', N'Xã Hiệp Phước', N'TP.HCM', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000135', N'Nhà số 136', N'Xã Thường Tân', N'Bình Dương', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000136', N'Nhà số 137', N'Xã Bắc Tân Uyên', N'Bình Dương', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000137', N'Nhà số 138', N'Xã Phú Giáo', N'Bình Dương', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000138', N'Nhà số 139', N'Xã Phước Hòa', N'Bình Dương', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000139', N'Nhà số 140', N'Xã Phước Thành', N'Bình Dương', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000140', N'Nhà số 141', N'Xã An Long', N'Bình Dương', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000141', N'Nhà số 142', N'Xã Trừ Văn Thố', N'Bình Dương', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000142', N'Nhà số 143', N'Xã Bàu Bàng', N'Bình Dương', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000143', N'Nhà số 144', N'Xã Long Hòa', N'Bình Dương', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000144', N'Nhà số 145', N'Xã Thanh An', N'Bình Dương', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000145', N'Nhà số 146', N'Xã Dầu Tiếng', N'Bình Dương', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000146', N'Nhà số 147', N'Xã Minh Thạnh', N'Bình Dương', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000147', N'Nhà số 148', N'Xã Châu Pha', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000148', N'Nhà số 149', N'Xã Long Hải', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000149', N'Nhà số 150', N'Xã Long Điền', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000150', N'Nhà số 151', N'Xã Phước Hải', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000151', N'Nhà số 152', N'Xã Đất Đỏ', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000152', N'Nhà số 153', N'Xã Nghĩa Thành', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000153', N'Nhà số 154', N'Xã Ngãi Giao', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000154', N'Nhà số 155', N'Xã Kim Long', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000155', N'Nhà số 156', N'Xã Châu Đức', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000156', N'Nhà số 157', N'Xã Bình Giã', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000157', N'Nhà số 158', N'Xã Xuân Sơn', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(9, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 9', '0900000158', N'Nhà số 159', N'Xã Hồ Tràm', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(10, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 10', '0900000159', N'Nhà số 160', N'Xã Xuyên Mộc', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(1, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 1', '0900000160', N'Nhà số 161', N'Xã Hòa Hội', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(2, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 2', '0900000161', N'Nhà số 162', N'Xã Bàu Lâm', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(3, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 3', '0900000162', N'Nhà số 163', N'Đặc khu Côn Đảo', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(4, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 4', '0900000163', N'Nhà số 164', N'Xã Bình Châu', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(5, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 5', '0900000164', N'Nhà số 165', N'Xã Hòa Hiệp', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(6, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 6', '0900000165', N'Nhà số 166', N'Xã Long Sơn', N'Bà Rịa - Vũng Tàu', 10.7, 106.6, 0, 0),
(7, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 7', '0900000166', N'Nhà số 167', N'Xã Thạnh An', N'TP.HCM', 10.7, 106.6, 0, 0),
(8, NULL, N'Địa chỉ sáp nhập', N'Khách Hàng 8', '0900000167', N'Nhà số 168', N'Phường Thới Hòa', N'Bình Dương', 10.7, 106.6, 0, 0);

/* =========================
   CẬP NHẬT LƯỢT BÁN CHO MÓN ĂN SAU KHI INSERT ĐƠN HÀNG
========================= */
UPDATE m
SET m.SoLuotBan = ISNULL((
    SELECT SUM(ct.SoLuongMua)
    FROM CT_DonHang ct
    JOIN DonHang d ON ct.MaDonHang = d.MaDonHang
    JOIN BienTheMonAn bt ON ct.MaBienThe = bt.MaBienThe
    WHERE d.TrangThaiDonHang = N'Hoàn thành' AND bt.MaMonAn = m.MaMonAn
), 0)
FROM MonAn m;
GO
