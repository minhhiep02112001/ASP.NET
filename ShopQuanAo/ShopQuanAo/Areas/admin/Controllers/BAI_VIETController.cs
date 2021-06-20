using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EntityFramework;

namespace ShopQuanAo.Areas.admin.Controllers
{
    public class BAI_VIETController : Controller
    {
        private ShopQuanAoDBContext db = new ShopQuanAoDBContext();

        // GET: admin/BAI_VIET
        public ActionResult Index()
        {
            return View(db.BAI_VIET.ToList());
        }

        // GET: admin/BAI_VIET/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAI_VIET bAI_VIET = db.BAI_VIET.Find(id);
            if (bAI_VIET == null)
            {
                return HttpNotFound();
            }
            return View(bAI_VIET);
        }

        // GET: admin/BAI_VIET/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/BAI_VIET/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MA_BV,TIEU_DE,MO_TA,SLUG,IMAGES,NOI_DUNG,NOI_BAT,TRANG_THAI,NGAY_DANG")] BAI_VIET bAI_VIET)
        {
            if (ModelState.IsValid)
            {
                db.BAI_VIET.Add(bAI_VIET);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bAI_VIET);
        }

        // GET: admin/BAI_VIET/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAI_VIET bAI_VIET = db.BAI_VIET.Find(id);
            if (bAI_VIET == null)
            {
                return HttpNotFound();
            }
            return View(bAI_VIET);
        }

        // POST: admin/BAI_VIET/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_BV,TIEU_DE,MO_TA,SLUG,IMAGES,NOI_DUNG,NOI_BAT,TRANG_THAI,NGAY_DANG")] BAI_VIET bAI_VIET)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bAI_VIET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bAI_VIET);
        }

        // GET: admin/BAI_VIET/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAI_VIET bAI_VIET = db.BAI_VIET.Find(id);
            if (bAI_VIET == null)
            {
                return HttpNotFound();
            }
            return View(bAI_VIET);
        }

        // POST: admin/BAI_VIET/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BAI_VIET bAI_VIET = db.BAI_VIET.Find(id);
            db.BAI_VIET.Remove(bAI_VIET);
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
