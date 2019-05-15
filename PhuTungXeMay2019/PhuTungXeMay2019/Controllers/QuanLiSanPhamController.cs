using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhuTungXeMay2019.Models;
using System.Transactions;

namespace PhuTungXeMay2019.Controllers
{
    public class QuanLiSanPhamController : Controller
    {
        CsK23T2bEntities db = new CsK23T2bEntities();

        // GET: /QuanLiSanPham/
        public ActionResult Index()
        {
            var model = db.Sanphams;
            return View(model.ToList());
        }

        // GET: /QuanLiSanPham/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // GET: /QuanLiSanPham/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /QuanLiSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Sanpham model)
        {
            ValidateSanpham(model);
            if (ModelState.IsValid)
            {
                using (var scope = TransactionScope())
                {
                    db.Sanphams.Add(model);
                    db.SaveChanges();
                    // save file to App_Data
                    var path = Server.MapPath("~/App_Data");
                    path = System.IO.Path.Combine(path, model.id.ToString());
                    Request.Files["Image"].SaveAs(path);
                    // accept all and persistence
                    scope.Complete();
                    return RedirectToAction("Index");
                }
               
            }

            return View(model);
        }
        public ActionResult Image(string id)
        {
            var path = Server.MapPath("~/App_Data");
            path = System.IO.Path.Combine(path, id);
            return File(path, "image/*");
        }
        private void ValidateSanpham(Sanpham model)
        {
            if (model.Gia <= 0)
                ModelState.AddModelError("Gia", SanPhamError.PRICE_LESS_0);
        }

        // GET: /QuanLiSanPham/Edit/5
        public ActionResult Edit(int id)
        {
            var model = db.Sanphams.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }

        // POST: /QuanLiSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Sanpham model)
        {
            ValidateSanpham(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /QuanLiSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // POST: /QuanLiSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sanpham sanpham = db.Sanphams.Find(id);
            db.Sanphams.Remove(sanpham);
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
