using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBanHangDoAnThucUong.Migrations
{
    public partial class NhanCapHeThong_Phan1_UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuongTrinhKhuyenMai",
                columns: table => new
                {
                    MaKMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChTrinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDungKMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TrangThaiKMai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuongTrinhKhuyenMai", x => x.MaKMai);
                });

            migrationBuilder.CreateTable(
                name: "DieuLe",
                columns: table => new
                {
                    MaDieuLe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDieuLe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhiChietKhau = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    NgayThemDieuLe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChiTietDieuLe = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieuLe", x => x.MaDieuLe);
                });

            migrationBuilder.CreateTable(
                name: "TheLoaiMonAn",
                columns: table => new
                {
                    MaLoaiMon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoaiMonAn", x => x.MaLoaiMon);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTaiKhoan);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_VaiTro_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoiTac",
                columns: table => new
                {
                    MaDoiTac = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuanDoiTac = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDTDoiTac = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    DiaChiDoiTac = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailDTac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiDoiTac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoiTac", x => x.MaDoiTac);
                    table.ForeignKey(
                        name: "FK_DoiTac_TaiKhoan_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GioiTinhKH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDTKH = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    PhuongXa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChiCuThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiemTichLuy = table.Column<int>(type: "int", nullable: false),
                    HangThanhVien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailKH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: true),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKH);
                    table.ForeignKey(
                        name: "FK_KhachHang_TaiKhoan_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GianHang",
                columns: table => new
                {
                    MaGianHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDoiTac = table.Column<int>(type: "int", nullable: false),
                    MaDieuLe = table.Column<int>(type: "int", nullable: false),
                    TenGianHang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhuongXa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChiCuThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioMo = table.Column<TimeSpan>(type: "time", nullable: false),
                    GioDong = table.Column<TimeSpan>(type: "time", nullable: false),
                    DanhGiaGianHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    TrangThaiGianHang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GianHang", x => x.MaGianHang);
                    table.ForeignKey(
                        name: "FK_GianHang_DieuLe_MaDieuLe",
                        column: x => x.MaDieuLe,
                        principalTable: "DieuLe",
                        principalColumn: "MaDieuLe",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GianHang_DoiTac_MaDoiTac",
                        column: x => x.MaDoiTac,
                        principalTable: "DoiTac",
                        principalColumn: "MaDoiTac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHang_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSuTimKiem",
                columns: table => new
                {
                    MaLSTimKiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<int>(type: "int", nullable: false),
                    TuKhoa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayTimKiem = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuTimKiem", x => x.MaLSTimKiem);
                    table.ForeignKey(
                        name: "FK_LichSuTimKiem_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<int>(type: "int", nullable: true),
                    MaGianHang = table.Column<int>(type: "int", nullable: false),
                    MaKMai = table.Column<int>(type: "int", nullable: true),
                    NgayTaoDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTienMon = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PhiDichVuSan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PhiShip = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ThanhTienKhachTra = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ThanhTienQuanNhan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TrangThaiDonHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LyDoHuy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_GianHang_MaGianHang",
                        column: x => x.MaGianHang,
                        principalTable: "GianHang",
                        principalColumn: "MaGianHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaGiamGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhanTramGiam = table.Column<int>(type: "int", nullable: false),
                    DieuKienApDung = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SuDung1Lan = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Kích hoạt"),
                    MaGianHang = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKhuyenMai);
                    table.ForeignKey(
                        name: "FK_KhuyenMai_GianHang_MaGianHang",
                        column: x => x.MaGianHang,
                        principalTable: "GianHang",
                        principalColumn: "MaGianHang");
                });

            migrationBuilder.CreateTable(
                name: "MonAn",
                columns: table => new
                {
                    MaMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGianHang = table.Column<int>(type: "int", nullable: false),
                    MaLoaiMon = table.Column<int>(type: "int", nullable: false),
                    TenMon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ThanhPhan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoSaoTrungBinh = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true, defaultValue: 0m),
                    SoLuotDanhGia = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    SoLuotBan = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonAn", x => x.MaMonAn);
                    table.ForeignKey(
                        name: "FK_MonAn_GianHang_MaGianHang",
                        column: x => x.MaGianHang,
                        principalTable: "GianHang",
                        principalColumn: "MaGianHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonAn_TheLoaiMonAn_MaLoaiMon",
                        column: x => x.MaLoaiMon,
                        principalTable: "TheLoaiMonAn",
                        principalColumn: "MaLoaiMon",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BienTheMonAn",
                columns: table => new
                {
                    MaBienThe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMonAn = table.Column<int>(type: "int", nullable: false),
                    MoTaMonAn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SoLuongMon = table.Column<int>(type: "int", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    HinhAnhMonAn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThaiMonAn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonAnMuaThem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienTheMonAn", x => x.MaBienThe);
                    table.ForeignKey(
                        name: "FK_BienTheMonAn_MonAn_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MonAn",
                        principalColumn: "MaMonAn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaMonAn",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMonAn = table.Column<int>(type: "int", nullable: true),
                    MaKH = table.Column<int>(type: "int", nullable: true),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    PhanHoiCuaDoiTac = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NgayPhanHoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiHienThi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaMonAn", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGiaMonAn_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaMonAn_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DanhGiaMonAn_MonAn_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MonAn",
                        principalColumn: "MaMonAn",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CT_DonHang",
                columns: table => new
                {
                    MaCTDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaBienThe = table.Column<int>(type: "int", nullable: false),
                    SoLuongMua = table.Column<int>(type: "int", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_DonHang", x => x.MaCTDonHang);
                    table.ForeignKey(
                        name: "FK_CT_DonHang_BienTheMonAn_MaBienThe",
                        column: x => x.MaBienThe,
                        principalTable: "BienTheMonAn",
                        principalColumn: "MaBienThe",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CT_DonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CT_GioHang",
                columns: table => new
                {
                    MaCTGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaBienThe = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    UocTinhThanhTien = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_GioHang", x => x.MaCTGioHang);
                    table.ForeignKey(
                        name: "FK_CT_GioHang_BienTheMonAn_MaBienThe",
                        column: x => x.MaBienThe,
                        principalTable: "BienTheMonAn",
                        principalColumn: "MaBienThe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CT_GioHang_GioHang_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienTheMonAn_MaMonAn",
                table: "BienTheMonAn",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_CT_DonHang_MaBienThe",
                table: "CT_DonHang",
                column: "MaBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_CT_DonHang_MaDonHang",
                table: "CT_DonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_CT_GioHang_MaBienThe",
                table: "CT_GioHang",
                column: "MaBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_CT_GioHang_MaGioHang",
                table: "CT_GioHang",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaMonAn_MaDonHang",
                table: "DanhGiaMonAn",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaMonAn_MaKH",
                table: "DanhGiaMonAn",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaMonAn_MaMonAn",
                table: "DanhGiaMonAn",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_DoiTac_MaTaiKhoan",
                table: "DoiTac",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaGianHang",
                table: "DonHang",
                column: "MaGianHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaKH",
                table: "DonHang",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_GianHang_MaDieuLe",
                table: "GianHang",
                column: "MaDieuLe");

            migrationBuilder.CreateIndex(
                name: "IX_GianHang_MaDoiTac",
                table: "GianHang",
                column: "MaDoiTac");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaKH",
                table: "GioHang",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_MaTaiKhoan",
                table: "KhachHang",
                column: "MaTaiKhoan",
                unique: true,
                filter: "[MaTaiKhoan] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_MaGiamGia",
                table: "KhuyenMai",
                column: "MaGiamGia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_MaGianHang",
                table: "KhuyenMai",
                column: "MaGianHang");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTimKiem_MaKH",
                table: "LichSuTimKiem",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_MaGianHang",
                table: "MonAn",
                column: "MaGianHang");

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_MaLoaiMon",
                table: "MonAn",
                column: "MaLoaiMon");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_MaVaiTro",
                table: "TaiKhoan",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_TenDangNhap",
                table: "TaiKhoan",
                column: "TenDangNhap",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropTable(
                name: "CT_DonHang");

            migrationBuilder.DropTable(
                name: "CT_GioHang");

            migrationBuilder.DropTable(
                name: "DanhGiaMonAn");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "LichSuTimKiem");

            migrationBuilder.DropTable(
                name: "BienTheMonAn");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "MonAn");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "GianHang");

            migrationBuilder.DropTable(
                name: "TheLoaiMonAn");

            migrationBuilder.DropTable(
                name: "DieuLe");

            migrationBuilder.DropTable(
                name: "DoiTac");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "VaiTro");
        }
    }
}
