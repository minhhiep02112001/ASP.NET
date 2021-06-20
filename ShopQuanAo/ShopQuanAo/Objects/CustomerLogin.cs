using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Objects
{
    public class CustomerLogin
    {
         [Required]
         public string TaiKhoan { get; set; }
         [Required]
         [MinLength(6)]
         public string MatKhau { get; set; }
         public bool? Remember { get; set; }
    }
}