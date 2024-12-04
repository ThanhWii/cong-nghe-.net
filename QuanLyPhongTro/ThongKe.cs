namespace QuanLyPhongTro
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ThongKe")]
    public partial class ThongKe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Since this is calculated, it's not auto-generated.
        public DateTime Thang { get; set; }

        public decimal TongTienThue { get; set; }

        public decimal TongTienNuoc { get; set; }

        public decimal TongTienDien { get; set; }

        [NotMapped] // Exclude this computed property from the database.
        public decimal ChiPhiBaoTri
        {
            get { return TongTienThue * 0.25m; }
        }
    }
}
