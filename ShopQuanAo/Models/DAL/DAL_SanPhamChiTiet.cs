using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL
{
    public class DAL_SanPhamChiTiet
    {
        private ShopQuanAoDBContext context = null;

        public DAL_SanPhamChiTiet()
        {
            this.context = new ShopQuanAoDBContext();
        }

        public bool insertSanPhamChiTiet(SAN_PHAM_CHI_TIET spct)
        {
            try
            {
                this.context.SAN_PHAM_CHI_TIET.Add(spct);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<SAN_PHAM_CHI_TIET> getListSanPhamDetails(string masp)
        {
            return this.context.SAN_PHAM_CHI_TIET.Where(a => a.MA_SP == masp).ToList();

        }
        public List<SAN_PHAM_CHI_TIET> getMaspAndIdColor(string masp , int idColor)
        {
            return this.context.SAN_PHAM_CHI_TIET.Where(a => a.MA_SP == masp && a.ID_COLOR == idColor).ToList();

        }

        public bool updateSPCT(int id, SAN_PHAM_CHI_TIET sp)
        {
            try
            {
                var spct = this.context.SAN_PHAM_CHI_TIET.Find(id);
                if (spct != null)
                {
                    spct.ID_COLOR = sp.ID_COLOR;
                    spct.MA_SP = sp.MA_SP;
                    spct.ID_SIZE = sp.ID_SIZE;
                    spct.SLUG = sp.SLUG;
                    spct.SO_LUONG = sp.SO_LUONG;
                    this.context.SaveChanges();
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SAN_PHAM_CHI_TIET returnSPCT(int id)
        {
            return this.context.SAN_PHAM_CHI_TIET.Find(id);
        }

         
    }
}
