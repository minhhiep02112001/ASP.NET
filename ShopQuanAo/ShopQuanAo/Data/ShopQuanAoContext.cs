﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Data
{
    public class ShopQuanAoContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ShopQuanAoContext() : base("name=ShopQuanAoContext")
        {
        }

        public System.Data.Entity.DbSet<Models.EntityFramework.BAI_VIET> BAI_VIET { get; set; }

        public System.Data.Entity.DbSet<Models.EntityFramework.SAN_PHAM> SAN_PHAM { get; set; }

        public System.Data.Entity.DbSet<Models.EntityFramework.SLIDE> SLIDEs { get; set; }

        public System.Data.Entity.DbSet<Models.EntityFramework.KHACH_HANG> KHACH_HANG { get; set; }

        public System.Data.Entity.DbSet<Models.EntityFramework.ACOUNT> ACOUNTs { get; set; }

        public System.Data.Entity.DbSet<Models.EntityFramework.HOA_DON> HOA_DON { get; set; }
    }
}
