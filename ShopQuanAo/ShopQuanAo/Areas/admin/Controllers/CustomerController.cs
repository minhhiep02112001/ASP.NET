using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DAL;
using Models.EntityFramework;
using PagedList;
using ShopQuanAo.Data;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class CustomerController : Controller
    {
        private ShopQuanAoContext db = new ShopQuanAoContext();

        // GET: admin/Customer
        private DAL_Customer dalCustomer { get; set; }
        public CustomerController()
        {
            this.dalCustomer = new DAL_Customer();
        }
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1) - 1;
            int totalCount = 0;
            var arrCustomer = this.dalCustomer.GetPageCustomers(pagenumber, 10, out totalCount);
            IPagedList<KHACH_HANG> pageOrders = new StaticPagedList<KHACH_HANG>(arrCustomer, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }

        // GET: admin/Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG kHACH_HANG = db.KHACH_HANG.Find(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // GET: admin/Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TEN_KH,SDT,EMAIL,MK,DIA_CHI")] KHACH_HANG kHACH_HANG)
        {
            try
            {

                var sql = "Select * from KHACH_HANG where EMAIL = N'" + kHACH_HANG.EMAIL + "';";
                var checksdt = this.dalCustomer.sqlQueryFristOrDefault("Select * from KHACH_HANG where SDT = N'" + kHACH_HANG.SDT + "';");
                var checkemail = this.dalCustomer.sqlQueryFristOrDefault(sql);
                if (ModelState.IsValid && checkemail == false)
                {
                    if (Request.Files.Get("images").ContentLength == 0)
                    {
                        ViewBag.Err = "Images không được để trống !!!";
                        return View(kHACH_HANG);
                    }
                    else
                    {
                        kHACH_HANG.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                    }
                    if (checksdt)
                    {
                        ViewBag.Err = "Số điện thoại đã tồn tại !!!";
                        return View(kHACH_HANG);
                    }
                    if (checkemail)
                    {
                        ViewBag.Err = "Email đã tồn tại !!!";
                        return View(kHACH_HANG);
                    }

                    if (!this.dalCustomer.InsertCustomers(kHACH_HANG))
                        {
                            ViewBag.Err = "Lỗi không thể chèn khách hàng !!!";
                            return View(kHACH_HANG);
                        }
                        else
                        {
                            ViewBag.Success = "Chèn Thành Công !!!";
                            return RedirectToAction("Index");
                        }
                    

                }
                // TODO: Add insert logic here
                
                    return View(kHACH_HANG);
                
            }
            catch
            {
                return View(kHACH_HANG);
            }
        }


        public string SaveUploadImage(HttpPostedFileBase file)
        {
            var str_file = "";
            Random random = new Random();
            if (file != null && file.ContentLength > 0)
            {
                //Định nghĩa đường dẫn lưu file trên server
                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadFileImage/images/") + file.FileName);
                string newFileName = file.FileName;
                //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                while (System.IO.File.Exists(ServerSavePath) == true)
                {
                    string filename = Path.GetFileNameWithoutExtension(newFileName);
                    string extension = Path.GetExtension(newFileName);
                    newFileName = filename + "_" + random.Next(1, 1000) + extension;
                    ServerSavePath = Path.Combine(Server.MapPath("~/UploadFileImage/images/") + newFileName);
                }
                //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                file.SaveAs(ServerSavePath);

                str_file = "/UploadFileImage/images/" + newFileName;
            }
            return str_file;// trả về đường dẫn file trong thư mục
        }

        // GET: admin/Customer/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var kHACH_HANG = this.dalCustomer.getCustomer(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // POST: admin/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TEN_KH,SDT,IMAGES,EMAIL,MK,DIA_CHI")] KHACH_HANG kHACH_HANG)
        {
            var sql = "Select * from KHACH_HANG where ID !='"+kHACH_HANG.ID+"' and EMAIL = N'" + kHACH_HANG.EMAIL + "';";
            var checksdt = this.dalCustomer.sqlQueryFristOrDefault("Select * from KHACH_HANG where ID !='" + kHACH_HANG.ID + "' and SDT = N'" + kHACH_HANG.SDT + "';");
            var checkemail = this.dalCustomer.sqlQueryFristOrDefault(sql);
            if (ModelState.IsValid && checkemail == false)
            {
                if (Request.Files.Get("images").ContentLength == 0)
                {
                    kHACH_HANG.IMAGES = this.dalCustomer.getCustomer(kHACH_HANG.ID).IMAGES;
                }
                else
                {
                    kHACH_HANG.IMAGES = SaveUploadImage(Request.Files.Get("images"));
                }
                if (checksdt)
                {
                    ViewBag.Err = "Số điện thoại đã tồn tại !!!";
                    return View(kHACH_HANG);
                }
                else
                {
                    if (!this.dalCustomer.EditCustomers(kHACH_HANG.ID ,kHACH_HANG))
                    {
                        ViewBag.Err = "Lỗi không thể chèn khách hàng !!!";
                        return View(kHACH_HANG);
                    }
                    else
                    {
                        ViewBag.Success = "Chèn Thành Công !!!";
                        return RedirectToAction("Index");
                    }
                }

            }
            // TODO: Add insert logic here
            else
            {
                ViewBag.Err = "Email đã tồn tại";
                return View(kHACH_HANG);
            }
        }

        // GET: admin/Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG kHACH_HANG = db.KHACH_HANG.Find(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // POST: admin/Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACH_HANG kHACH_HANG = db.KHACH_HANG.Find(id);
            db.KHACH_HANG.Remove(kHACH_HANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
