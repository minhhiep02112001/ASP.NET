using Models.DAL;
using Models.EntityFramework;
using PagedList;
using ShopQuanAo.Areas.admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private ShopQuanAoDBContext context = null;

        public HomeController()
        {
            this.context = new ShopQuanAoDBContext();
        }

        private void Share()
        {
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0);
            ViewBag.parentCategory = parentCategory;
        }

        [ChildActionOnly]
        public ActionResult LoadParentMenu()
        {
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0).ToList();
            return PartialView("_LoadParentMenu", parentCategory);
        }

        [ChildActionOnly]
        public ActionResult LoadChilden( int parentID)
        {
            var childenCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == parentID).ToList();
            ViewBag.Count = childenCategory.Count();
            return PartialView("_LoadChilden", childenCategory);
        }


        [ChildActionOnly]
        public ActionResult LoadPopularProducts()
        {
            var arrCategory = new List<LOAI_SAN_PHAM>();
            var parentCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == 0).ToList();

            if (parentCategory.Count() > 0)
            {
                foreach (var item in parentCategory)
                {
                    var listArrCategory = new List<LOAI_SAN_PHAM>();
                    listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory , (int)item.ID_LOAI_SP);
                    if (listArrCategory.Count > 0)
                    {
                        foreach (var key in listArrCategory)
                        {
                            var arr = this.context.SAN_PHAM.Where(c => c.ID_LSP == key.ID_LOAI_SP && c.TRANG_THAI == true).ToList();
                            if (arr.Count() > 0)
                            {
                                arrCategory.Add(item);
                            }

                        }
                    }
                }
            }
            return PartialView("_LoadPopularProducts", arrCategory);
        }

        [ChildActionOnly]
        public ActionResult LoadContentPopularProduct(int id)
        {
            var listProduct = new List<SAN_PHAM>();
            var listArrCategory = new List<LOAI_SAN_PHAM>();
            listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory, id);

            if(listArrCategory.Count() > 0)
            {
                foreach(var item in listArrCategory)
                {
                    var arr = this.context.SAN_PHAM.Where(c => c.ID_LSP == item.ID_LOAI_SP && c.TRANG_THAI == true).ToList();
                    if (arr.Count() > 0)
                    {
                        foreach(var i in arr)
                        {
                            listProduct.Add(i);
                        }
                    }
                }
            }
            return PartialView("_LoadContentPopularProduct" , listProduct);
        }

        [ChildActionOnly]
        public ActionResult LoadSlides()
        {
            var arrSlides = this.context.SLIDEs.Where(a => a.TRANG_THAI == true);
            return PartialView("_LoadSlides", arrSlides);
        }

        [ChildActionOnly]
        public ActionResult LoadProductNew()
        {
            // Show 15 sản phẩm mới nhất
            var arrProduct = this.context.SAN_PHAM.Where(a => a.TRANG_THAI == true).OrderBy(c=>c.NGAY_TAO).Skip(0).Take(15);
            return PartialView("_LoadProductNew", arrProduct);
        }

        [ChildActionOnly]
        public ActionResult LoadPosts()
        {
            // Show 15 sản phẩm mới nhất
            var arrBaiViet = this.context.BAI_VIET.Where(a => a.TRANG_THAI == true && a.NOI_BAT == true).OrderBy(c => c.NGAY_DANG).Skip(0).Take(3);
            return PartialView("_LoadPosts", arrBaiViet);
        }

       
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [ChildActionOnly]
        public ActionResult LoadMenuLeftDanhMuc(int parent_id , int id)
        {
            var dequy = new DeQuyCategory();
            var dm = new LOAI_SAN_PHAM();
            var data  = new List<LOAI_SAN_PHAM>();
            if(parent_id != 0)
            {
                dequy.getParentLoaiSanPham(data, parent_id);
                foreach (var i in data)
                {
                    if (i.ID_CHA == 0)
                    {
                        dm = i;
                        break;
                    }
                }
            }
            else
            {
                dm = this.context.LOAI_SAN_PHAM.Find(id);
            }
            ViewBag.MenuParent = dm;
            var listChildrenDanhMuc = this.context.LOAI_SAN_PHAM.Select(a=>a).ToList();
            return PartialView("_LoadMenuLeftDanhMuc", listChildrenDanhMuc);
        }

        [ChildActionOnly]
        public ActionResult LoadMenuLeftChildrenDanhMuc(int parentID)
        {
            var childenCategory = this.context.LOAI_SAN_PHAM.Where(a => a.ID_CHA == parentID).ToList();
            ViewBag.CountChildrenDanhMuc = childenCategory.Count();
            return PartialView("_LoadMenuLeftChildrenDanhMuc", childenCategory);
        }

        [HttpGet]
        public ActionResult DanhMuc(string id , int? page , int pageSize = 10)
        {
            var danhmuc = this.context.LOAI_SAN_PHAM.Where(a => a.SLUG == id).FirstOrDefault();
            
            if (danhmuc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Danhmuc = danhmuc;
            var listArrCategory = new List<LOAI_SAN_PHAM>();
            listArrCategory.Add(danhmuc);
            listArrCategory = new DeQuyCategory().getChildenLoaiSanPham(listArrCategory, (int)danhmuc.ID_LOAI_SP);
            var str = "";
            if(listArrCategory.Count() > 0)
            {
                var arr = new List<string>();
                foreach(var item in listArrCategory)
                {
                    arr.Add(item.ID_LOAI_SP.ToString());
                }
                str = string.Join(",", arr.ToArray());
            }
            int pagenumber = (page ?? 1);
            string sql = String.Format("Select * FROM SAN_PHAM WHERE SAN_PHAM.ID_LSP IN({0})",str);
            var arrSanPham = this.context.SAN_PHAM.SqlQuery(sql).ToPagedList(pagenumber, pageSize);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(arrSanPham);
        }


        [ChildActionOnly]
        public ActionResult LoadColor(string id)
        {
            var arrColor = new List<COLOR>();
            arrColor = this.context.COLORs.Where(c => c.MA_SP == id).ToList();
            return PartialView("_LoadColor", arrColor);
        }

        [ChildActionOnly]
        public ActionResult LoadImageProductDetalis(string id)
        {
            var arrImg = new List<string>();
            var sp = this.context.SAN_PHAM.Find(id);
            if(sp != null)
            {
                arrImg.Add(sp.LINK_ANH_CHINH);
                foreach(var i in sp.LIST_ANH_KEM.Split(','))
                {
                    arrImg.Add(i);
                }
                var arrColor = this.context.COLORs.Where(c => c.MA_SP == id);
                foreach(var item in arrColor)
                {
                    arrImg.Add(item.IMAGES);
                }
            }
            ViewBag.ArrImage = arrImg;
            return PartialView("_LoadImageProductDetalis");
        }

        /* "/sanpham/ao-da-calic"*/

        [ChildActionOnly]
        public ActionResult LoadProductNoiBat()
        {
            var arrSanPham = this.context.SAN_PHAM.Where(a => a.NOI_BAT == true && a.TRANG_THAI == true).OrderBy(c=>c.NGAY_TAO).Skip(0).Take(12);
            return PartialView("_LoadProductNoiBat", arrSanPham);
        }
        [HttpGet]
        public ActionResult SanPham(string id)
        {
            var product = this.context.SAN_PHAM.Where(a => a.SLUG == id).FirstOrDefault();
            if(product == null)
            {
                var id_product = this.context.SAN_PHAM_CHI_TIET.Where(a => a.SLUG == id).FirstOrDefault();
                if(id_product!= null)
                {
                    product = this.context.SAN_PHAM.Where(a => a.MA_SP == id_product.MA_SP).FirstOrDefault();
                }
            }
            if(product == null)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(product);
        }
        
        [HttpGet]
        public ActionResult BaiViet(int page = 1)
        {
            var arrBaiViet =this.context.BAI_VIET.Where(a=>a.TRANG_THAI== true).OrderBy(c=>c.NGAY_DANG).ToPagedList(page , 12);
            return View(arrBaiViet);
        }
        [HttpGet]
        public ActionResult ChiTietBaiViet(string id)
        {
            var baiviet = this.context.BAI_VIET.Where(a => a.SLUG == id).FirstOrDefault();
            if(baiviet == null)
            {
                return RedirectToAction("BaiViet", "Home");
            }
            return View(baiviet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}