using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyPhongTro
{
    public partial class QLPhongTro : DbContext
    {
        public QLPhongTro()
            : base("name=QLPhongTro")
        {
        }
        
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public virtual DbSet<Dien> Dien { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<KhachThue> KhachThue { get; set; }
        public virtual DbSet<Nuoc> Nuoc { get; set; }
        public virtual DbSet<Phong> Phong { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(e => e.TienPhong)
                .HasPrecision(10, 3);

            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(e => e.TienDien)
                .HasPrecision(10, 3);

            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(e => e.TienNuoc)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Dien>()
                .Property(e => e.ChiSoCu)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Dien>()
                .Property(e => e.ChiSoMoi)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Dien>()
                .Property(e => e.GiaTien)
                .HasPrecision(10, 3);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.ThanhTien)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Nuoc>()
                .Property(e => e.ChiSoCu)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Nuoc>()
                .Property(e => e.ChiSoMoi)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Nuoc>()
                .Property(e => e.GiaTien)
                .HasPrecision(10, 3);

            modelBuilder.Entity<Phong>()
                .Property(e => e.GiaThueThang)
                .HasPrecision(10, 3);
        }
    }
}
