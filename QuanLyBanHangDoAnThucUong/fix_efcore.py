with open('Data/ApplicationDbContext.cs', 'r', encoding='utf-8') as f:
    text = f.read()

insert_str = """
            modelBuilder.Entity<DonHang>()
                .Property(d => d.TongGiaTri)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<DonHang>()
                .Property(d => d.KinhDoGiao)
                .HasPrecision(18, 6);

            modelBuilder.Entity<DonHang>()
                .Property(d => d.ViDoGiao)
                .HasPrecision(18, 6);
"""

text = text.replace('            modelBuilder.Entity<DonHang>()\n                .Property(d => d.TongGiaTri)\n                .HasColumnType("decimal(18,2)");', insert_str)

with open('Data/ApplicationDbContext.cs', 'w', encoding='utf-8') as f:
    f.write(text)

print("Fixed ApplicationDbContext EF Core warnings.")
