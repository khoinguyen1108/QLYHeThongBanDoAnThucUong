DECLARE @MaDL INT = 1;

-- Highlands
INSERT INTO GianHang (MaDoiTac, MaDieuLe, TenGianHang, DiaChiCuThe, PhuongXa, ThanhPho, GioMo, GioDong, TrangThaiGianHang)
VALUES 
(1, @MaDL, N'Highlands Coffee - Landmark 81', N'Tầng trệt Landmark 81', N'Phường 22', N'Bình Thạnh', '07:00', '22:00', N'Mở cửa'),
(1, @MaDL, N'Highlands Coffee - Dinh Độc Lập', N'135 Nam Kỳ Khởi Nghĩa', N'Bến Thành', N'Quận 1', '07:00', '22:00', N'Mở cửa');

-- Phuc Long
INSERT INTO GianHang (MaDoiTac, MaDieuLe, TenGianHang, DiaChiCuThe, PhuongXa, ThanhPho, GioMo, GioDong, TrangThaiGianHang)
VALUES 
(2, @MaDL, N'Phúc Long - Ngô Đức Kế', N'29 Ngô Đức Kế', N'Bến Nghé', N'Quận 1', '07:00', '22:30', N'Mở cửa'),
(2, @MaDL, N'Phúc Long - GigaMall', N'Phạm Văn Đồng', N'Hiệp Bình Chánh', N'Thủ Đức', '07:00', '22:30', N'Mở cửa');

-- Gong Cha
INSERT INTO GianHang (MaDoiTac, MaDieuLe, TenGianHang, DiaChiCuThe, PhuongXa, ThanhPho, GioMo, GioDong, TrangThaiGianHang)
VALUES 
(3, @MaDL, N'Gong Cha - Hồ Tùng Mậu', N'83 Hồ Tùng Mậu', N'Bến Nghé', N'Quận 1', '09:00', '22:00', N'Mở cửa'),
(3, @MaDL, N'Gong Cha - Phan Xích Long', N'240 Phan Xích Long', N'Phường 7', N'Phú Nhuận', '09:00', '22:00', N'Mở cửa');
