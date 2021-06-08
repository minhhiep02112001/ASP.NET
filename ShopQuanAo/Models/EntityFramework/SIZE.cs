namespace Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SIZE")]
    public partial class SIZE
    {
        public int ID { get; set; }

        [StringLength(15)]
        public string TEN_SIZE { get; set; }

        [StringLength(20)]
        public string SLUG { get; set; }

        [StringLength(50)]
        public string MO_TA { get; set; }

        public bool? TRANG_THAI { get; set; }
    }
}
