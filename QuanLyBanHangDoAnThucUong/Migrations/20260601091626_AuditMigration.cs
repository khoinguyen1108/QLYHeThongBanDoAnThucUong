using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBanHangDoAnThucUong.Migrations
{
    public partial class AuditMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_DoiTac_MaDoiTac",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMai_GianHang_MaGianHang",
                table: "KhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_DoiTac_MaDoiTac",
                table: "MonAn");

            migrationBuilder.RenameColumn(
                name: "MaDoiTac",
                table: "MonAn",
                newName: "MaGianHang");

            migrationBuilder.RenameIndex(
                name: "IX_MonAn_MaDoiTac",
                table: "MonAn",
                newName: "IX_MonAn_MaGianHang");

            migrationBuilder.RenameColumn(
                name: "DaThanhToan",
                table: "DonHang",
                newName: "ViDoGiao");

            migrationBuilder.RenameColumn(
                name: "NgayKThuc",
                table: "ChuongTrinhKhuyenMai",
                newName: "NgayKetThuc");

            migrationBuilder.RenameColumn(
                name: "NgayBDau",
                table: "ChuongTrinhKhuyenMai",
                newName: "NgayBatDau");

            migrationBuilder.RenameColumn(
                name: "MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                newName: "MaGianHang");

            migrationBuilder.RenameIndex(
                name: "IX_ChuongTrinhKhuyenMai_MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                newName: "IX_ChuongTrinhKhuyenMai_MaGianHang");

            migrationBuilder.AlterColumn<string>(
                name: "ThanhPho",
                table: "KhachHang",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HangThanhVien",
                table: "KhachHang",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "DiaChiMaDiaChi",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KinhDoGiao",
                table: "DonHang",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MaDiaChi",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiNhan",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiNhan",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PhanTramGiam",
                table: "ChuongTrinhKhuyenMai",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "GiamToiDa",
                table: "ChuongTrinhKhuyenMai",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LoaiKhuyenMai",
                table: "ChuongTrinhKhuyenMai",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiaChi",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKh = table.Column<int>(type: "int", nullable: true),
                    MaGianHang = table.Column<int>(type: "int", nullable: true),
                    TenGoiNho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoaiNhan = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DiaChiCuThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhuongXa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ViDo = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    KinhDo = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    LaMacDinh = table.Column<bool>(type: "bit", nullable: true),
                    DaXoa = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChi", x => x.MaDiaChi);
                    table.ForeignKey(
                        name: "FK_DiaChi_GianHang_MaGianHang",
                        column: x => x.MaGianHang,
                        principalTable: "GianHang",
                        principalColumn: "MaGianHang",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DiaChi_KhachHang_MaKh",
                        column: x => x.MaKh,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TinNhan",
                columns: table => new
                {
                    MaTinNhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiGui = table.Column<int>(type: "int", nullable: true),
                    NguoiNhan = table.Column<int>(type: "int", nullable: true),
                    MaDanhGia = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaXem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhan", x => x.MaTinNhan);
                    table.ForeignKey(
                        name: "FK_TinNhan_TaiKhoan_NguoiGui",
                        column: x => x.NguoiGui,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTaiKhoan");
                    table.ForeignKey(
                        name: "FK_TinNhan_TaiKhoan_NguoiNhan",
                        column: x => x.NguoiNhan,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTaiKhoan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_DiaChiMaDiaChi",
                table: "DonHang",
                column: "DiaChiMaDiaChi");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChi_MaGianHang",
                table: "DiaChi",
                column: "MaGianHang");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChi_MaKh",
                table: "DiaChi",
                column: "MaKh");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhan_NguoiGui",
                table: "TinNhan",
                column: "NguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhan_NguoiNhan",
                table: "TinNhan",
                column: "NguoiNhan");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_GianHang_MaGianHang",
                table: "ChuongTrinhKhuyenMai",
                column: "MaGianHang",
                principalTable: "GianHang",
                principalColumn: "MaGianHang",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_DiaChi_DiaChiMaDiaChi",
                table: "DonHang",
                column: "DiaChiMaDiaChi",
                principalTable: "DiaChi",
                principalColumn: "MaDiaChi");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMai_GianHang_MaGianHang",
                table: "KhuyenMai",
                column: "MaGianHang",
                principalTable: "GianHang",
                principalColumn: "MaGianHang",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_GianHang_MaGianHang",
                table: "MonAn",
                column: "MaGianHang",
                principalTable: "GianHang",
                principalColumn: "MaGianHang",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_GianHang_MaGianHang",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_DiaChi_DiaChiMaDiaChi",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMai_GianHang_MaGianHang",
                table: "KhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_GianHang_MaGianHang",
                table: "MonAn");

            migrationBuilder.DropTable(
                name: "DiaChi");

            migrationBuilder.DropTable(
                name: "TinNhan");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_DiaChiMaDiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "DiaChiMaDiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "KinhDoGiao",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "MaDiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiNhan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "TenNguoiNhan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GiamToiDa",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropColumn(
                name: "LoaiKhuyenMai",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.RenameColumn(
                name: "MaGianHang",
                table: "MonAn",
                newName: "MaDoiTac");

            migrationBuilder.RenameIndex(
                name: "IX_MonAn_MaGianHang",
                table: "MonAn",
                newName: "IX_MonAn_MaDoiTac");

            migrationBuilder.RenameColumn(
                name: "ViDoGiao",
                table: "DonHang",
                newName: "DaThanhToan");

            migrationBuilder.RenameColumn(
                name: "NgayKetThuc",
                table: "ChuongTrinhKhuyenMai",
                newName: "NgayKThuc");

            migrationBuilder.RenameColumn(
                name: "NgayBatDau",
                table: "ChuongTrinhKhuyenMai",
                newName: "NgayBDau");

            migrationBuilder.RenameColumn(
                name: "MaGianHang",
                table: "ChuongTrinhKhuyenMai",
                newName: "MaDoiTac");

            migrationBuilder.RenameIndex(
                name: "IX_ChuongTrinhKhuyenMai_MaGianHang",
                table: "ChuongTrinhKhuyenMai",
                newName: "IX_ChuongTrinhKhuyenMai_MaDoiTac");

            migrationBuilder.AlterColumn<string>(
                name: "ThanhPho",
                table: "KhachHang",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HangThanhVien",
                table: "KhachHang",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PhanTramGiam",
                table: "ChuongTrinhKhuyenMai",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_DoiTac_MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                column: "MaDoiTac",
                principalTable: "DoiTac",
                principalColumn: "MaDoiTac",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMai_GianHang_MaGianHang",
                table: "KhuyenMai",
                column: "MaGianHang",
                principalTable: "GianHang",
                principalColumn: "MaGianHang");

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_DoiTac_MaDoiTac",
                table: "MonAn",
                column: "MaDoiTac",
                principalTable: "DoiTac",
                principalColumn: "MaDoiTac",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
