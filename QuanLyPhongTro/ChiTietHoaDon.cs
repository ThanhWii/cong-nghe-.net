namespace QuanLyPhongTro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietHoaDon")]
    public partial class ChiTietHoaDon
    {
        [Key]
        public int MaChiTiet { get; set; }

        public int? MaHoaDon { get; set; }

        public int? MaPhong { get; set; }

        public decimal TienPhong { get; set; }

        public decimal TienDien { get; set; }

        public decimal TienNuoc { get; set; }

        public virtual HoaDon HoaDon { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
