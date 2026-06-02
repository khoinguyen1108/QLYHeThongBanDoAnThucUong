using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyBanHangDoAnThucUong.Migrations
{
    public partial class AddDiaChiGiaoHangToDonHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_GianHang_MaGianHang",
                table: "MonAn");

            migrationBuilder.RenameColumn(
                name: "MaGianHang",
                table: "MonAn",
                newName: "MaDoiTac");

            migrationBuilder.RenameIndex(
                name: "IX_MonAn_MaGianHang",
                table: "MonAn",
                newName: "IX_MonAn_MaDoiTac");

            migrationBuilder.AlterColumn<string>(
                name: "TenLoai",
                table: "TheLoaiMonAn",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TenMon",
                table: "MonAn",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "EmailKH",
                table: "KhachHang",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiGianHang",
                table: "GianHang",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TenGianHang",
                table: "GianHang",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiCuThe",
                table: "GianHang",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<decimal>(
                name: "DaThanhToan",
                table: "DonHang",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiGiaoHang",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenQuanDoiTac",
                table: "DoiTac",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiDoiTac",
                table: "DoiTac",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaMonAn",
                table: "BienTheMonAn",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnhMonAn",
                table: "BienTheMonAn",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_ChuongTrinhKhuyenMai_MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                column: "MaDoiTac");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_DoiTac_MaDoiTac",
                table: "ChuongTrinhKhuyenMai",
                column: "MaDoiTac",
                principalTable: "DoiTac",
                principalColumn: "MaDoiTac",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_DoiTac_MaDoiTac",
                table: "MonAn",
                column: "MaDoiTac",
                principalTable: "DoiTac",
                principalColumn: "MaDoiTac",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuongTrinhKhuyenMai_DoiTac_MaDoiTac",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_DoiTac_MaDoiTac",
                table: "MonAn");

            migrationBuilder.DropIndex(
                name: "IX_ChuongTrinhKhuyenMai_MaDoiTac",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.DropColumn(
                name: "DaThanhToan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHang",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "MaDoiTac",
                table: "ChuongTrinhKhuyenMai");

            migrationBuilder.RenameColumn(
                name: "MaDoiTac",
                table: "MonAn",
                newName: "MaGianHang");

            migrationBuilder.RenameIndex(
                name: "IX_MonAn_MaDoiTac",
                table: "MonAn",
                newName: "IX_MonAn_MaGianHang");

            migrationBuilder.AlterColumn<string>(
                name: "TenLoai",
                table: "TheLoaiMonAn",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenMon",
                table: "MonAn",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailKH",
                table: "KhachHang",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiGianHang",
                table: "GianHang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenGianHang",
                table: "GianHang",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiCuThe",
                table: "GianHang",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenQuanDoiTac",
                table: "DoiTac",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiDoiTac",
                table: "DoiTac",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaMonAn",
                table: "BienTheMonAn",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnhMonAn",
                table: "BienTheMonAn",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_GianHang_MaGianHang",
                table: "MonAn",
                column: "MaGianHang",
                principalTable: "GianHang",
                principalColumn: "MaGianHang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
