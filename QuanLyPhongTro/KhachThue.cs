namespace QuanLyPhongTro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachThue")]
    public partial class KhachThue
    {
        [Key]
        public int MaKhachThue { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTenDem { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten { get; set; }

        [StringLength(100)]
        public string CMND { get; set; }

        [StringLength(20)]
        public string DienThoai { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayVaoO { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTraPhong { get; set; }

        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
