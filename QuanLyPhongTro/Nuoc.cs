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
        [Required(ErrorMessage = "Bạn chưa nhập ngày đọc số.")]
        public DateTime NgayDocSo { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập chỉ số cũ.")]
        public decimal ChiSoCu { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập chỉ số mới.")]
        public decimal ChiSoMoi { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập giá tiền.")]
        public decimal GiaTien { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn mã phòng.")]
        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
