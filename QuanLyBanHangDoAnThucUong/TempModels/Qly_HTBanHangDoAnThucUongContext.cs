using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLyBanHangDoAnThucUong.TempModels
{
    public partial class Qly_HTBanHangDoAnThucUongContext : DbContext
    {
        public Qly_HTBanHangDoAnThucUongContext()
        {
        }

        public Qly_HTBanHangDoAnThucUongContext(DbContextOptions<Qly_HTBanHangDoAnThucUongContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BienTheMonAn> BienTheMonAns { get; set; } = null!;
        public virtual DbSet<ChuongTrinhKhuyenMai> ChuongTrinhKhuyenMais { get; set; } = null!;
        public virtual DbSet<CtDonHang> CtDonHangs { get; set; } = null!;
        public virtual DbSet<CtGioHang> CtGioHangs { get; set; } = null!;
        public virtual DbSet<DanhGiaMonAn> DanhGiaMonAns { get; set; } = null!;
        public virtual DbSet<DiaChi> DiaChis { get; set; } = null!;
        public virtual DbSet<DieuLe> DieuLes { get; set; } = null!;
        public virtual DbSet<DoiTac> DoiTacs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<GianHang> GianHangs { get; set; } = null!;
        public virtual DbSet<GiaoHang> GiaoHangs { get; set; } = null!;
        public virtual DbSet<GioHang> GioHangs { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<LichSuDonHang> LichSuDonHangs { get; set; } = null!;
        public virtual DbSet<LichSuXemMon> LichSuXemMons { get; set; } = null!;
        public virtual DbSet<MonAn> MonAns { get; set; } = null!;
        public virtual DbSet<NhanVienHeThong> NhanVienHeThongs { get; set; } = null!;
        public virtual DbSet<PhuongThucThToan> PhuongThucThToans { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
        public virtual DbSet<ThanhToan> ThanhToans { get; set; } = null!;
        public virtual DbSet<TheLoaiMonAn> TheLoaiMonAns { get; set; } = null!;
        public virtual DbSet<TinNhan> TinNhans { get; set; } = null!;
        public virtual DbSet<VaiTro> VaiTros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Qly_HTBanHangDoAnThucUong;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BienTheMonAn>(entity =>
            {
                entity.HasKey(e => e.MaBienThe)
                    .HasName("PK__BienTheM__3987CEF51D24BF0E");

                entity.ToTable("BienTheMonAn");

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.GiaBan).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.HinhAnhMonAn).HasMaxLength(255);

                entity.Property(e => e.MoTaMonAn).HasMaxLength(200);

                entity.Property(e => e.MonAnMuaThem).HasMaxLength(100);

                entity.Property(e => e.SoLuongMon).HasDefaultValueSql("((0))");

                entity.Property(e => e.TrangThaiMonAn).HasMaxLength(20);

                entity.HasOne(d => d.MaMonAnNavigation)
                    .WithMany(p => p.BienTheMonAns)
                    .HasForeignKey(d => d.MaMonAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BienTheMo__MaMon__4F7CD00D");
            });

            modelBuilder.Entity<ChuongTrinhKhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaKmai)
                    .HasName("PK__ChuongTr__47526CA6F7B4C7D1");

                entity.ToTable("ChuongTrinhKhuyenMai");

                entity.Property(e => e.MaKmai).HasColumnName("MaKMai");

                entity.Property(e => e.GiamToiDa).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.LoaiKhuyenMai)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'HeThong')");

                entity.Property(e => e.NgayBatDau)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.Property(e => e.NoiDungKmai)
                    .HasMaxLength(200)
                    .HasColumnName("NoiDungKMai");

                entity.Property(e => e.TenChTrinh).HasMaxLength(100);

                entity.Property(e => e.TrangThaiKmai)
                    .HasMaxLength(200)
                    .HasColumnName("TrangThaiKMai");

                entity.HasOne(d => d.MaGianHangNavigation)
                    .WithMany(p => p.ChuongTrinhKhuyenMais)
                    .HasForeignKey(d => d.MaGianHang)
                    .HasConstraintName("FK__ChuongTri__MaGia__440B1D61");
            });

            modelBuilder.Entity<CtDonHang>(entity =>
            {
                entity.HasKey(e => e.MaCtdonHang)
                    .HasName("PK__CT_DonHa__B9F4AB6C89B56401");

                entity.ToTable("CT_DonHang");

                entity.Property(e => e.MaCtdonHang).HasColumnName("MaCTDonHang");

                entity.Property(e => e.GiaBan).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.MaBienTheNavigation)
                    .WithMany(p => p.CtDonHangs)
                    .HasForeignKey(d => d.MaBienThe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CT_DonHan__MaBie__0F624AF8");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.CtDonHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CT_DonHan__MaDon__0E6E26BF");
            });

            modelBuilder.Entity<CtGioHang>(entity =>
            {
                entity.HasKey(e => e.MaCtgioHang)
                    .HasName("PK__CT_GioHa__CE44847311A7F259");

                entity.ToTable("CT_GioHang");

                entity.Property(e => e.MaCtgioHang).HasColumnName("MaCTGioHang");

                entity.Property(e => e.GiaBan).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UocTinhThanhTien).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.MaBienTheNavigation)
                    .WithMany(p => p.CtGioHangs)
                    .HasForeignKey(d => d.MaBienThe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CT_GioHan__MaBie__6D0D32F4");

                entity.HasOne(d => d.MaGioHangNavigation)
                    .WithMany(p => p.CtGioHangs)
                    .HasForeignKey(d => d.MaGioHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CT_GioHan__MaGio__6C190EBB");
            });

            modelBuilder.Entity<DanhGiaMonAn>(entity =>
            {
                entity.HasKey(e => e.MaDanhGia)
                    .HasName("PK__DanhGiaM__AA9515BF62F70E0A");

                entity.ToTable("DanhGiaMonAn");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.NgayDanhGia)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoiDung).HasMaxLength(500);

                entity.Property(e => e.PhanHoiCuaDoiTac).HasMaxLength(1000);

                entity.Property(e => e.TrangThaiHienThi)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.DanhGiaMonAns)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DanhGiaMo__MaDon__1DB06A4F");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.DanhGiaMonAns)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__DanhGiaMon__MaKH__1CBC4616");

                entity.HasOne(d => d.MaMonAnNavigation)
                    .WithMany(p => p.DanhGiaMonAns)
                    .HasForeignKey(d => d.MaMonAn)
                    .HasConstraintName("FK__DanhGiaMo__MaMon__1BC821DD");
            });

            modelBuilder.Entity<DiaChi>(entity =>
            {
                entity.HasKey(e => e.MaDiaChi)
                    .HasName("PK__DiaChi__EB61213E9BAB1E1A");

                entity.ToTable("DiaChi");

                entity.Property(e => e.DaXoa).HasDefaultValueSql("((0))");

                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);

                entity.Property(e => e.KinhDo).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.LaMacDinh).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.PhuongXa).HasMaxLength(100);

                entity.Property(e => e.SoDienThoaiNhan)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.TenGoiNho).HasMaxLength(100);

                entity.Property(e => e.TenNguoiNhan).HasMaxLength(100);

                entity.Property(e => e.ThanhPho).HasMaxLength(100);

                entity.Property(e => e.ViDo).HasColumnType("decimal(9, 6)");

                entity.HasOne(d => d.MaGianHangNavigation)
                    .WithMany(p => p.DiaChis)
                    .HasForeignKey(d => d.MaGianHang)
                    .HasConstraintName("FK__DiaChi__MaGianHa__5EBF139D");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.DiaChis)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__DiaChi__MaKH__5DCAEF64");
            });

            modelBuilder.Entity<DieuLe>(entity =>
            {
                entity.HasKey(e => e.MaDieuLe)
                    .HasName("PK__DieuLe__3A85E2B91A1B0C9A");

                entity.ToTable("DieuLe");

                entity.Property(e => e.ChiTietDieuLe).HasMaxLength(500);

                entity.Property(e => e.NgayThemDieuLe)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhiChietKhau).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.TenDieuLe).HasMaxLength(200);
            });

            modelBuilder.Entity<DoiTac>(entity =>
            {
                entity.HasKey(e => e.MaDoiTac)
                    .HasName("PK__DoiTac__5F76BF344A8348B1");

                entity.ToTable("DoiTac");

                entity.Property(e => e.DiaChiDoiTac).HasMaxLength(255);

                entity.Property(e => e.EmailDtac)
                    .HasMaxLength(255)
                    .HasColumnName("EmailDTac");

                entity.Property(e => e.SoDtdoiTac)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SoDTDoiTac");

                entity.Property(e => e.TenQuanDoiTac).HasMaxLength(100);

                entity.Property(e => e.TrangThaiDoiTac).HasMaxLength(80);

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.DoiTacs)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DoiTac__MaTaiKho__30F848ED");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang)
                    .HasName("PK__DonHang__129584AD38A81D28");

                entity.ToTable("DonHang");

                entity.Property(e => e.DiaChiGiaoHang).HasMaxLength(500);

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.GiamGia).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.KinhDoGiao).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.LyDoHuy).HasMaxLength(500);

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.MaKmai).HasColumnName("MaKMai");

                entity.Property(e => e.NgayTaoDon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhiDichVuSan).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PhiShip).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SoDienThoaiNhan)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.TenNguoiNhan).HasMaxLength(100);

                entity.Property(e => e.ThanhTienKhachTra).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ThanhTienQuanNhan).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TongTienMon).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TrangThaiDonHang)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Chá» xÃ¡c nháº­n')");

                entity.Property(e => e.TrangThaiThanhToan)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ChÆ°a thanh toÃ¡n')");

                entity.Property(e => e.ViDoGiao).HasColumnType("decimal(9, 6)");

                entity.HasOne(d => d.MaDiaChiNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaDiaChi)
                    .HasConstraintName("FK__DonHang__MaDiaCh__7F2BE32F");

                entity.HasOne(d => d.MaGianHangNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaGianHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonHang__MaGianH__7D439ABD");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__DonHang__MaKH__7C4F7684");

                entity.HasOne(d => d.MaKmaiNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaKmai)
                    .HasConstraintName("FK__DonHang__MaKMai__7E37BEF6");
            });

            modelBuilder.Entity<GianHang>(entity =>
            {
                entity.HasKey(e => e.MaGianHang)
                    .HasName("PK__GianHang__1772B231713590BB");

                entity.ToTable("GianHang");

                entity.Property(e => e.DanhGiaGianHang).HasMaxLength(275);

                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);

                entity.Property(e => e.HinhAnh).HasMaxLength(255);

                entity.Property(e => e.LuotXem).HasDefaultValueSql("((0))");

                entity.Property(e => e.PhuongXa).HasMaxLength(100);

                entity.Property(e => e.TenGianHang).HasMaxLength(200);

                entity.Property(e => e.ThanhPho).HasMaxLength(255);

                entity.Property(e => e.TrangThaiGianHang).HasMaxLength(20);

                entity.HasOne(d => d.MaDieuLeNavigation)
                    .WithMany(p => p.GianHangs)
                    .HasForeignKey(d => d.MaDieuLe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GianHang__MaDieu__3A81B327");

                entity.HasOne(d => d.MaDoiTacNavigation)
                    .WithMany(p => p.GianHangs)
                    .HasForeignKey(d => d.MaDoiTac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GianHang__MaDoiT__398D8EEE");
            });

            modelBuilder.Entity<GiaoHang>(entity =>
            {
                entity.HasKey(e => e.MaGiaoHang)
                    .HasName("PK__GiaoHang__81CCF4FDFBA774E5");

                entity.ToTable("GiaoHang");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.PhiVanChuyen).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TgianDuKienDen).HasColumnName("TGianDuKienDen");

                entity.Property(e => e.TgianXuatPhat).HasColumnName("TGianXuatPhat");

                entity.Property(e => e.TrangThaiGhang)
                    .HasMaxLength(50)
                    .HasColumnName("TrangThaiGHang");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.GiaoHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GiaoHang__MaDonH__03F0984C");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.GiaoHangs)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GiaoHang__MaNV__04E4BC85");
            });

            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.HasKey(e => e.MaGioHang)
                    .HasName("PK__GioHang__F5001DA36935A46D");

                entity.ToTable("GioHang");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.GioHangs)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__GioHang__MaKH__693CA210");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KhachHan__2725CF1E42992C3F");

                entity.ToTable("KhachHang");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);

                entity.Property(e => e.DiemTichLuy).HasDefaultValueSql("((0))");

                entity.Property(e => e.EmailKh)
                    .HasMaxLength(255)
                    .HasColumnName("EmailKH");

                entity.Property(e => e.GioiTinhKh)
                    .HasMaxLength(10)
                    .HasColumnName("GioiTinhKH")
                    .IsFixedLength();

                entity.Property(e => e.HangThanhVien)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'ThÆ°á»ng')");

                entity.Property(e => e.NgayDangKy)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhuongXa).HasMaxLength(100);

                entity.Property(e => e.SoDtkh)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SoDTKH");

                entity.Property(e => e.TenKh)
                    .HasMaxLength(100)
                    .HasColumnName("TenKH");

                entity.Property(e => e.ThanhPho).HasMaxLength(255);

                entity.Property(e => e.ThuHangBac).HasMaxLength(100);

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.KhachHangs)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .HasConstraintName("FK__KhachHang__MaTai__5812160E");
            });

            modelBuilder.Entity<LichSuDonHang>(entity =>
            {
                entity.HasKey(e => e.MaLichSu)
                    .HasName("PK__LichSuDo__C443222A37911D36");

                entity.ToTable("LichSuDonHang");

                entity.Property(e => e.ThoiGian)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThaiDonHang)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Chá» xÃ¡c nháº­n')");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.LichSuDonHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichSuDon__MaDon__151B244E");
            });

            modelBuilder.Entity<LichSuXemMon>(entity =>
            {
                entity.HasKey(e => e.Ma)
                    .HasName("PK__LichSuXe__3214CC9F473D4E23");

                entity.ToTable("LichSuXemMon");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.ThoiGian)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.LichSuXemMons)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__LichSuXemM__MaKH__2180FB33");

                entity.HasOne(d => d.MaMonAnNavigation)
                    .WithMany(p => p.LichSuXemMons)
                    .HasForeignKey(d => d.MaMonAn)
                    .HasConstraintName("FK__LichSuXem__MaMon__22751F6C");
            });

            modelBuilder.Entity<MonAn>(entity =>
            {
                entity.HasKey(e => e.MaMonAn)
                    .HasName("PK__MonAn__B1171625EBE6B818");

                entity.ToTable("MonAn");

                entity.Property(e => e.SoLuotBan).HasDefaultValueSql("((0))");

                entity.Property(e => e.SoLuotDanhGia).HasDefaultValueSql("((0))");

                entity.Property(e => e.SoSaoTrungBinh)
                    .HasColumnType("decimal(3, 1)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TenMon).HasMaxLength(200);

                entity.Property(e => e.ThanhPhan).HasMaxLength(500);

                entity.HasOne(d => d.MaGianHangNavigation)
                    .WithMany(p => p.MonAns)
                    .HasForeignKey(d => d.MaGianHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MonAn__MaGianHan__49C3F6B7");

                entity.HasOne(d => d.MaLoaiMonNavigation)
                    .WithMany(p => p.MonAns)
                    .HasForeignKey(d => d.MaLoaiMon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MonAn__MaLoaiMon__4AB81AF0");
            });

            modelBuilder.Entity<NhanVienHeThong>(entity =>
            {
                entity.HasKey(e => e.MaNv)
                    .HasName("PK__NhanVien__2725D70A943EB5A6");

                entity.ToTable("NhanVienHeThong");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.DiaChiTtru)
                    .HasMaxLength(255)
                    .HasColumnName("DiaChiTTru");

                entity.Property(e => e.EmailNv)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EmailNV");

                entity.Property(e => e.GioiTinhNv)
                    .HasMaxLength(5)
                    .HasColumnName("GioiTinhNV")
                    .IsFixedLength();

                entity.Property(e => e.SoDtnv)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SoDTNV");

                entity.Property(e => e.TenNv)
                    .HasMaxLength(100)
                    .HasColumnName("TenNV");

                entity.Property(e => e.TrangThaiLamViec)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Äang lÃ m')");

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.NhanVienHeThongs)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NhanVienH__MaTai__66603565");
            });

            modelBuilder.Entity<PhuongThucThToan>(entity =>
            {
                entity.HasKey(e => e.MaPtthanhToan)
                    .HasName("PK__PhuongTh__7DD7B6F6511B6F17");

                entity.ToTable("PhuongThucThToan");

                entity.Property(e => e.MaPtthanhToan).HasColumnName("MaPTThanhToan");

                entity.Property(e => e.TenPhuongThuc).HasMaxLength(150);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan)
                    .HasName("PK__TaiKhoan__AD7C652993A194C5");

                entity.ToTable("TaiKhoan");

                entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC0E4F2A94D")
                    .IsUnique();

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.TenDangNhap).HasMaxLength(100);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Hoáº¡t Ä‘á»™ng')");

                entity.HasOne(d => d.MaVaiTroNavigation)
                    .WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.MaVaiTro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaiKhoan__MaVaiT__2C3393D0");
            });

            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaThanhToan)
                    .HasName("PK__ThanhToa__D4B25844892E03CC");

                entity.ToTable("ThanhToan");

                entity.Property(e => e.GhiChuThanhToan).HasMaxLength(500);

                entity.Property(e => e.MaPtthanhToan).HasColumnName("MaPTThanhToan");

                entity.Property(e => e.NgayThanhToan)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoTienThanhToan).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TrangThaiThanhToan)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(N'ÄÃ£ thanh toÃ¡n')");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhToan__MaDon__0A9D95DB");

                entity.HasOne(d => d.MaPtthanhToanNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaPtthanhToan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhToan__MaPTT__0B91BA14");
            });

            modelBuilder.Entity<TheLoaiMonAn>(entity =>
            {
                entity.HasKey(e => e.MaLoaiMon)
                    .HasName("PK__TheLoaiM__612C5AE46C059A91");

                entity.ToTable("TheLoaiMonAn");

                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<TinNhan>(entity =>
            {
                entity.HasKey(e => e.MaTinNhan)
                    .HasName("PK__TinNhan__E5B3062A9E3A3FC5");

                entity.ToTable("TinNhan");

                entity.Property(e => e.ThoiGianGui)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NguoiGuiNavigation)
                    .WithMany(p => p.TinNhanNguoiGuiNavigations)
                    .HasForeignKey(d => d.NguoiGui)
                    .HasConstraintName("FK_TinNhan_NguoiGui");

                entity.HasOne(d => d.NguoiNhanNavigation)
                    .WithMany(p => p.TinNhanNguoiNhanNavigations)
                    .HasForeignKey(d => d.NguoiNhan)
                    .HasConstraintName("FK_TinNhan_NguoiNhan");
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasKey(e => e.MaVaiTro)
                    .HasName("PK__VaiTro__C24C41CFF4D97920");

                entity.ToTable("VaiTro");

                entity.Property(e => e.TenVaiTro).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
