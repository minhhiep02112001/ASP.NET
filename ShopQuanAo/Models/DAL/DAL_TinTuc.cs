using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
namespace Models.DAL
{
    public class DAL_TinTuc
    {
        private ShopQuanAoDBContext context { get; set; }
        public DAL_TinTuc()
        {
            this.context = new ShopQuanAoDBContext();
        }

        public List<BAI_VIET> GetPageSlides(int page, int itemsPerPage, out int totalCount)
        {
            var list = new List<BAI_VIET>();
            var arrTinTuc = this.context.BAI_VIET.Select(a => a).OrderBy(c => c.NGAY_DANG).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            foreach (var item in arrTinTuc)
            {
                list.Add(item);
            }
            totalCount = this.context.BAI_VIET.Count();
            return list;
        }

        public bool sqlQueryFristOrDefault(string sql)
        {
            var sp = this.context.BAI_VIET.SqlQuery(sql).FirstOrDefault();
            if (sp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateTinTuc (BAI_VIET baiviet)
        {
            try
            {
                baiviet.NGAY_DANG = DateTime.Now;
                baiviet.NOI_BAT = false;
                baiviet.TRANG_THAI = false;
                this.context.BAI_VIET.Add(baiviet);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EditTinTuc(int id , BAI_VIET baiviet)
        {
            try
            {
                var tintuc = this.context.BAI_VIET.Find(id);
                if(tintuc != null)
                {
                    tintuc.IMAGES = baiviet.IMAGES;
                    tintuc.MO_TA = baiviet.MO_TA;
                    tintuc.NOI_DUNG = baiviet.NOI_DUNG;
                    tintuc.SLUG = baiviet.SLUG;
                    tintuc.TIEU_DE = baiviet.TIEU_DE;
                    this.context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTinTuc (int ma)
        {
            try
            {
                var tintuc = this.context.BAI_VIET.Find(ma);
                if(tintuc != null)
                {
                    this.context.BAI_VIET.Remove(tintuc);
                }                return true;
            }
            catch
            {
                return false;
            }
        }

        public BAI_VIET returnBaiViet(int ma)
        {
            return this.context.BAI_VIET.Find(ma);
        }
    }
}
