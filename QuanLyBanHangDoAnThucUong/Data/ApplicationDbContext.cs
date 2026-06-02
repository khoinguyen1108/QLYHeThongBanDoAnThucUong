using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Models.Entities;

namespace QuanLyBanHangDoAnThucUong.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<DieuLe> DieuLes { get; set; }
        public DbSet<DoiTac> DoiTacs { get; set; }
        public DbSet<GianHang> GianHangs { get; set; }
        public DbSet<TheLoaiMonAn> TheLoaiMonAns { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<BienTheMonAn> BienTheMonAns { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DiaChi> DiaChis { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<CT_GioHang> CT_GioHangs { get; set; }
        public DbSet<ChuongTrinhKhuyenMai> ChuongTrinhKhuyenMais { get; set; }
        
        public DbSet<DanhGiaMonAn> DanhGiaMonAns { get; set; }
        public DbSet<LichSuTimKiem> LichSuTimKiems { get; set; }
        public DbSet<TinNhan> TinNhans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== MAP TABLE NAME =====
            modelBuilder.Entity<VaiTro>().ToTable("VaiTro");
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<DieuLe>().ToTable("DieuLe");
            modelBuilder.Entity<DoiTac>().ToTable("DoiTac");
            modelBuilder.Entity<GianHang>().ToTable("GianHang");
            modelBuilder.Entity<MonAn>().ToTable("MonAn");
            modelBuilder.Entity<BienTheMonAn>().ToTable("BienTheMonAn");
            modelBuilder.Entity<TheLoaiMonAn>().ToTable("TheLoaiMonAn");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang");
            modelBuilder.Entity<DonHang>().ToTable("DonHang");
            modelBuilder.Entity<ChiTietDonHang>().ToTable("CT_DonHang");
            modelBuilder.Entity<GioHang>().ToTable("GioHang");
            modelBuilder.Entity<CT_GioHang>().ToTable("CT_GioHang");
            modelBuilder.Entity<ChuongTrinhKhuyenMai>().ToTable("ChuongTrinhKhuyenMai");
            modelBuilder.Entity<TinNhan>().ToTable("TinNhan");
            modelBuilder.Entity<DiaChi>().ToTable("DiaChi");

            // ===== VaiTro =====
            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasKey(e => e.MaVaiTro);
                entity.Property(e => e.TenVaiTro).IsRequired().HasMaxLength(100);
            });

            // ===== TaiKhoan =====
            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan);
                entity.Property(e => e.TenDangNhap).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.Property(e => e.MatKhau).IsRequired();

                // TaiKhoan -> VaiTro (Many-to-One)
                entity.HasOne(e => e.VaiTro)
                    .WithMany(v => v.TaiKhoans)
                    .HasForeignKey(e => e.MaVaiTro)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== DieuLe =====
            modelBuilder.Entity<DieuLe>(entity =>
            {
                entity.HasKey(e => e.MaDieuLe);
                entity.Property(e => e.TenDieuLe).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PhiChietKhau).HasPrecision(5, 2);
            });

            // ===== DoiTac =====
            modelBuilder.Entity<DoiTac>(entity =>
            {
                entity.HasKey(e => e.MaDoiTac);
                entity.Property(e => e.TenQuanDoiTac).HasMaxLength(100);
                entity.Property(e => e.SoDTDoiTac).HasMaxLength(11);
                entity.Property(e => e.DiaChiDoiTac).HasMaxLength(255);

                // DoiTac -> TaiKhoan (One-to-One)
                entity.HasOne(e => e.TaiKhoan)
                    .WithOne(t => t.DoiTac)
                    .HasForeignKey<DoiTac>(e => e.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.Cascade);

                // DoiTac -> GianHang (One-to-Many)
                entity.HasMany(e => e.GianHangs)
                    .WithOne(g => g.DoiTac)
                    .HasForeignKey(g => g.MaDoiTac)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== GianHang =====
            modelBuilder.Entity<GianHang>(entity =>
            {
                entity.HasKey(e => e.MaGianHang);
                entity.Property(e => e.TenGianHang).HasMaxLength(200);
                entity.Property(e => e.PhuongXa).HasMaxLength(100);
                entity.Property(e => e.ThanhPho).HasMaxLength(100);
                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);

                // GianHang -> DoiTac (Many-to-One)
                entity.HasOne(e => e.DoiTac)
                    .WithMany(d => d.GianHangs)
                    .HasForeignKey(e => e.MaDoiTac)
                    .OnDelete(DeleteBehavior.Restrict);

                // GianHang -> DieuLe (Many-to-One)
                entity.HasOne(e => e.DieuLe)
                    .WithMany(d => d.GianHangs)
                    .HasForeignKey(e => e.MaDieuLe)
                    .OnDelete(DeleteBehavior.Restrict);


            });

            // ===== TheLoaiMonAn =====
            modelBuilder.Entity<TheLoaiMonAn>(entity =>
            {
                entity.HasKey(e => e.MaLoaiMon);
                entity.Property(e => e.TenLoai).HasMaxLength(100);

                // TheLoaiMonAn -> MonAn (One-to-Many)
                entity.HasMany(e => e.MonAns)
                    .WithOne(m => m.TheLoaiMonAn)
                    .HasForeignKey(m => m.MaLoaiMon)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== MonAn =====
            modelBuilder.Entity<MonAn>(entity =>
            {
                entity.HasKey(e => e.MaMonAn);
                entity.Property(e => e.TenMon).HasMaxLength(200);
                entity.Property(e => e.ThanhPhan).HasMaxLength(500);
                entity.Property(e => e.SoSaoTrungBinh).HasPrecision(3, 1).HasDefaultValue(0m);
                entity.Property(e => e.SoLuotDanhGia).HasDefaultValue(0);

                // MonAn -> GianHang (Many-to-One)
                entity.HasOne(e => e.GianHang)
                    .WithMany(g => g.MonAns)
                    .HasForeignKey(e => e.MaGianHang)
                    .OnDelete(DeleteBehavior.Cascade);

                // MonAn -> TheLoaiMonAn (Many-to-One)
                entity.HasOne(e => e.TheLoaiMonAn)
                    .WithMany(t => t.MonAns)
                    .HasForeignKey(e => e.MaLoaiMon)
                    .OnDelete(DeleteBehavior.Restrict);

                // MonAn -> BienTheMonAn (One-to-Many)
                entity.HasMany(e => e.BienTheMonAns)
                    .WithOne(b => b.MonAn)
                    .HasForeignKey(b => b.MaMonAn)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== BienTheMonAn =====
            modelBuilder.Entity<BienTheMonAn>(entity =>
            {
                entity.HasKey(e => e.MaBienThe);
                entity.Property(e => e.MoTaMonAn).HasMaxLength(200);
                entity.Property(e => e.GiaBan).HasPrecision(10, 2);
                entity.Property(e => e.HinhAnhMonAn).HasMaxLength(255);

                // BienTheMonAn -> MonAn (Many-to-One)
                entity.HasOne(e => e.MonAn)
                    .WithMany(m => m.BienTheMonAns)
                    .HasForeignKey(e => e.MaMonAn)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== KhachHang =====
            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKH);
                entity.Property(e => e.TenKH).HasMaxLength(100);
                entity.Property(e => e.SoDTKH).HasMaxLength(11);
                entity.Property(e => e.EmailKH).HasMaxLength(255);
                entity.Property(e => e.PhuongXa).HasMaxLength(100);
                entity.Property(e => e.ThanhPho).HasMaxLength(255);
                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);

                // KhachHang -> TaiKhoan (One-to-One, Optional)
                entity.HasOne(e => e.TaiKhoan)
                    .WithOne(t => t.KhachHang)
                    .HasForeignKey<KhachHang>(e => e.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== DonHang =====
            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang);
                entity.Property(e => e.TongTienMon).HasPrecision(10, 2);
                entity.Property(e => e.GiamGia).HasPrecision(10, 2);
                entity.Property(e => e.PhiDichVuSan).HasPrecision(10, 2);
                entity.Property(e => e.PhiShip).HasPrecision(10, 2);
                entity.Property(e => e.ThanhTienKhachTra).HasPrecision(10, 2);
                entity.Property(e => e.ThanhTienQuanNhan).HasPrecision(10, 2);
                entity.Property(e => e.TrangThaiDonHang).IsRequired().HasMaxLength(50);

                // DonHang -> KhachHang (Many-to-One)
                entity.HasOne(e => e.KhachHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaKH)
                    .OnDelete(DeleteBehavior.Restrict);

                // DonHang -> GianHang (Many-to-One)
                entity.HasOne(e => e.GianHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaGianHang)
                    .OnDelete(DeleteBehavior.Restrict);

                // DonHang -> DiaChi (Many-to-One) — khai báo tường minh tránh shadow property
                entity.HasOne<DiaChi>()
                    .WithMany(d => d.DonHangs)
                    .HasForeignKey(e => e.MaDiaChi)
                    .OnDelete(DeleteBehavior.SetNull);

                // DonHang -> ChiTietDonHang (One-to-Many)
                entity.HasMany(e => e.ChiTietDonHangs)
                    .WithOne(c => c.DonHang)
                    .HasForeignKey(c => c.MaDonHang)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== ChiTietDonHang =====
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.MaCTDonHang);
                entity.Property(e => e.GiaBan).HasPrecision(10, 2);

                // ChiTietDonHang -> DonHang (Many-to-One)
                entity.HasOne(e => e.DonHang)
                    .WithMany(d => d.ChiTietDonHangs)
                    .HasForeignKey(e => e.MaDonHang)
                    .OnDelete(DeleteBehavior.Cascade);

                // ChiTietDonHang -> BienTheMonAn (Many-to-One)
                entity.HasOne(e => e.BienTheMonAn)
                    .WithMany()
                    .HasForeignKey(e => e.MaBienThe)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== GioHang =====
            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.HasKey(e => e.MaGioHang);

                // GioHang -> KhachHang (Many-to-One)
                entity.HasOne(e => e.KhachHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaKH)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // GioHang -> CT_GioHang (One-to-Many)
                entity.HasMany(e => e.CT_GioHangs)
                    .WithOne(c => c.GioHang)
                    .HasForeignKey(c => c.MaGioHang)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CT_GioHang =====
            modelBuilder.Entity<CT_GioHang>(entity =>
            {
                entity.HasKey(e => e.MaCTGioHang);
                entity.Property(e => e.GiaBan).HasPrecision(10, 2);
                entity.Property(e => e.UocTinhThanhTien).HasPrecision(10, 2);

                // CT_GioHang -> GioHang (Many-to-One)
                // configured in GioHang section

                // CT_GioHang -> BienTheMonAn (Many-to-One)
                entity.HasOne(e => e.BienTheMonAn)
                    .WithMany()
                    .HasForeignKey(e => e.MaBienThe)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== ChuongTrinhKhuyenMai =====
            modelBuilder.Entity<ChuongTrinhKhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaKMai);
                // entity.Property(e => e.PhanTramGiam).HasConversion<double>();

                // ChuongTrinhKhuyenMai -> GianHang (Many-to-One)
                entity.HasOne(e => e.GianHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaGianHang)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== ChuongTrinhKhuyenMai =====
            modelBuilder.Entity<ChuongTrinhKhuyenMai>(entity =>
            {
                entity.ToTable("ChuongTrinhKhuyenMai");
                entity.HasKey(e => e.MaKMai);
                entity.Property(e => e.TenChTrinh).IsRequired().HasMaxLength(255);
                entity.Property(e => e.NoiDungKMai).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.NoiDungKMai).IsUnique();
                entity.Property(e => e.GiamToiDa).HasPrecision(10, 2);
                entity.Property(e => e.TrangThaiKMai).HasMaxLength(50).HasDefaultValue("Kích hoạt");

                // Quan hệ ChuongTrinhKhuyenMai -> GianHang
                entity.HasOne(e => e.GianHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaGianHang)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== DanhGiaMonAn =====
            modelBuilder.Entity<DanhGiaMonAn>(entity =>
            {
                entity.ToTable("DanhGiaMonAn");
                entity.HasKey(e => e.MaDanhGia);
                entity.Property(e => e.NoiDung).HasMaxLength(500);
                entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.MonAn)
                    .WithMany()
                    .HasForeignKey(e => e.MaMonAn)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.DonHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaDonHang)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.KhachHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaKH)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== LichSuTimKiem =====
            modelBuilder.Entity<LichSuTimKiem>(entity =>
            {
                entity.ToTable("LichSuTimKiem");
                entity.HasKey(e => e.MaLSTimKiem);
                entity.Property(e => e.TuKhoa).HasMaxLength(200);
                entity.Property(e => e.NgayTimKiem).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.KhachHang)
                    .WithMany()
                    .HasForeignKey(e => e.MaKH)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== DiaChi =====
            modelBuilder.Entity<DiaChi>(entity =>
            {
                entity.HasKey(e => e.MaDiaChi);
                entity.Property(e => e.TenNguoiNhan).HasMaxLength(100);
                entity.Property(e => e.SoDienThoaiNhan).HasMaxLength(11);
                entity.Property(e => e.DiaChiCuThe).HasMaxLength(255);
                entity.Property(e => e.PhuongXa).HasMaxLength(100);
                entity.Property(e => e.ThanhPho).HasMaxLength(100);
                entity.Property(e => e.ViDo).HasPrecision(9, 6);
                entity.Property(e => e.KinhDo).HasPrecision(9, 6);

                // DiaChi -> KhachHang
                entity.HasOne(e => e.MaKhNavigation)
                    .WithMany()
                    .HasForeignKey(e => e.MaKh)
                    .OnDelete(DeleteBehavior.SetNull);

                // DiaChi -> GianHang
                entity.HasOne(e => e.MaGianHangNavigation)
                    .WithMany()
                    .HasForeignKey(e => e.MaGianHang)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
