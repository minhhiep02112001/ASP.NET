namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BAI_VIET
    {
        [Key]
        public int MA_BV { get; set; }

        [Required]
        [StringLength(100)]
        public string TIEU_DE { get; set; }

        [StringLength(255)]
        public string MO_TA { get; set; }

        [Required]
        [StringLength(120)]
        public string SLUG { get; set; }

        [StringLength(100)]
        public string IMAGES { get; set; }

        [Column(TypeName = "text")]
        public string NOI_DUNG { get; set; }

        public bool? NOI_BAT { get; set; }

        public bool? TRANG_THAI { get; set; }

        public DateTime? NGAY_DANG { get; set; }
    }
}
