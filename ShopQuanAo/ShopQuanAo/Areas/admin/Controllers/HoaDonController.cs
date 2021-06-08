using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DAL;
using Models.EntityFramework;
using ShopQuanAo.Data;
using Models.ADO;
using PagedList;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class HoaDonController : Controller
    {
     
        private DAL_Bill dalBill { get; set; }

        public HoaDonController()
        {
            this.dalBill = new DAL_Bill();
        }

        // GET: admin/HoaDon
        public ActionResult Index(int? page  , string search , string t , string s , string d , string p , string g , string status)
        {
            int pagenumber = (page ?? 1) - 1;
            var str_search = "";

            // ?search=1&t=hiep&s=0394599501&d=ba vì&p=1200000&g=đẹp&status=1
            if (!string.IsNullOrEmpty(search))
            {
                str_search += " MA_HD LIKE '%" + search +"%'";  
            }
            if (!string.IsNullOrEmpty(t))
            {
                str_search += " and TEN_NHAN_HANG LIKE N'%" + t + "%'";
            }
            if (!string.IsNullOrEmpty(s))
            {
                str_search += " and SDT_NHAN LIKE N'%" + s + "%'";
            }
            if (!string.IsNullOrEmpty(d))
            {
                str_search += " and DIA_CHI_NHAN LIKE N'%" + d + "%'";
            }
            if (!string.IsNullOrEmpty(p))
            {
                str_search += " and TONG_TIEN LIKE N'%" + p + "%'";
            }
            if (!string.IsNullOrEmpty(g))
            {
                str_search += " and GHI_CHU LIKE N'%" + g + "%'";
            }

            if (!string.IsNullOrEmpty(status))
            {
                str_search += " and TRANG_THAI LIKE '%" + status + "%'";
            }

            if (str_search != "")
            {
                str_search = str_search.Trim();
                if (str_search.IndexOf("and") == 0)
                {
                    str_search = str_search.Remove(0, 3);
                }
            }

            int totalCount = 0;
            var listBill = this.dalBill.GetPageBills( str_search , pagenumber, 10, out totalCount);
            IPagedList<ModelBills> pageOrders = new StaticPagedList<ModelBills>(listBill, pagenumber + 1, 10, totalCount);
            return View(pageOrders);
        }


        public JsonResult changeStatus(int id , FormCollection collection)
        {
            var status =int.Parse( collection["status"]);
            var check = this.dalBill.ChangeStatus(id, status);
            return Json(new
            {
                status = check
            }, JsonRequestBehavior.AllowGet); 
        }

        // GET: admin/HoaDon/Details/5
        /*      public ActionResult Details(int? id)
              {
                  if (id == null)
                  {
                      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                  }
                  HOA_DON hOA_DON = db.HOA_DON.Find(id);
                  if (hOA_DON == null)
                  {
                      return HttpNotFound();
                  }
                  return View(hOA_DON);
              }

              // GET: admin/HoaDon/Create
              public ActionResult Create()
              {
                  return View();
              }

              // POST: admin/HoaDon/Create
              // To protect from overposting attacks, enable the specific properties you want to bind to, for 
              // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
              [HttpPost]
              [ValidateAntiForgeryToken]
              public ActionResult Create([Bind(Include = "MA_HD,ID_KH,NGAY_MUA,DIA_CHI_NHAN,SDT_NHAN,TONG_TIEN,TRANG_THAI,PT_THANH_TOAN")] HOA_DON hOA_DON)
              {
                  if (ModelState.IsValid)
                  {
                      db.HOA_DON.Add(hOA_DON);
                      db.SaveChanges();
                      return RedirectToAction("Index");
                  }

                  return View(hOA_DON);
              }

              // GET: admin/HoaDon/Edit/5
              public ActionResult Edit(int? id)
              {
                  if (id == null)
                  {
                      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                  }
                  HOA_DON hOA_DON = db.HOA_DON.Find(id);
                  if (hOA_DON == null)
                  {
                      return HttpNotFound();
                  }
                  return View(hOA_DON);
              }

              // POST: admin/HoaDon/Edit/5
              // To protect from overposting attacks, enable the specific properties you want to bind to, for 
              // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
              [HttpPost]
              [ValidateAntiForgeryToken]
              public ActionResult Edit([Bind(Include = "MA_HD,ID_KH,NGAY_MUA,DIA_CHI_NHAN,SDT_NHAN,TONG_TIEN,TRANG_THAI,PT_THANH_TOAN")] HOA_DON hOA_DON)
              {
                  if (ModelState.IsValid)
                  {
                      db.Entry(hOA_DON).State = EntityState.Modified;
                      db.SaveChanges();
                      return RedirectToAction("Index");
                  }
                  return View(hOA_DON);
              }

              // GET: admin/HoaDon/Delete/5
              public ActionResult Delete(int? id)
              {
                  if (id == null)
                  {
                      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                  }
                  HOA_DON hOA_DON = db.HOA_DON.Find(id);
                  if (hOA_DON == null)
                  {
                      return HttpNotFound();
                  }
                  return View(hOA_DON);
              }

              // POST: admin/HoaDon/Delete/5
              [HttpPost, ActionName("Delete")]
              [ValidateAntiForgeryToken]
              public ActionResult DeleteConfirmed(int id)
              {
                  HOA_DON hOA_DON = db.HOA_DON.Find(id);
                  db.HOA_DON.Remove(hOA_DON);
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
              }*/
    }
}
