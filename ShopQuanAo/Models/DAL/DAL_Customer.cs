using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL
{
    public class DAL_Customer
    {
        protected ShopQuanAoDBContext context { get; set; }
        public DAL_Customer()
        {
            this.context = new ShopQuanAoDBContext();
        }

        public List<KHACH_HANG> GetPageCustomers(int page, int itemsPerPage, out int totalCount)
        {
            var list = new List<KHACH_HANG>();
            var arrKH = this.context.KHACH_HANG.Select(a => a).OrderBy(c => c.NGAY_TAO).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            foreach (var item in arrKH)
            {
                list.Add(item);
            }
            totalCount = this.context.KHACH_HANG.Count();
            return list;
        }

        public bool sqlQueryFristOrDefault(string sql)
        {
            var kh = this.context.KHACH_HANG.SqlQuery(sql).FirstOrDefault();
            if (kh != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertCustomers(KHACH_HANG kh)
        {
            try
            {
                kh.NGAY_TAO = DateTime.Now;
                kh.TRANG_THAI = false;
                this.context.KHACH_HANG.Add(kh);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EditCustomers(int id, KHACH_HANG kh)
        {
            try
            {
                var khachhang = this.context.KHACH_HANG.Find(id);
                if (khachhang != null)
                {
                    khachhang.SDT = kh.SDT;
                    khachhang.DIA_CHI = kh.DIA_CHI;
                    khachhang.EMAIL = kh.EMAIL;
                    khachhang.IMAGES = kh.IMAGES;
                    khachhang.TEN_KH = kh.TEN_KH;
                    khachhang.MK = kh.MK;
                    this.context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public KHACH_HANG getCustomer(int id)
        {
            return this.context.KHACH_HANG.Find(id);
        }
    }
}
