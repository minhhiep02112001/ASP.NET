namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KHACH_HANG
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TEN_KH { get; set; }

        [Required]
        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(200)]
        public string IMAGES { get; set; }

        [Required]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(100)]
        public string MK { get; set; }

        [Required]
        [StringLength(50)]
        public string DIA_CHI { get; set; }

        public bool? TRANG_THAI { get; set; }

        public DateTime? NGAY_TAO { get; set; }
    }
}
