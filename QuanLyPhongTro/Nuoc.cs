namespace QuanLyPhongTro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Nuoc")]
    public partial class Nuoc
    {
        [Key]
        public int MaNuoc { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayDocSo { get; set; }

        public decimal ChiSoCu { get; set; }

        public decimal ChiSoMoi { get; set; }

        public decimal GiaTien { get; set; }

        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
