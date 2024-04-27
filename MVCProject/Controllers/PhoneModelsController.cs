using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MVCProject.Controllers
{
    public class PhoneModelsController : Controller
    {
        private GadgetDBContext db = new GadgetDBContext();
        public ActionResult Index()
        {
            return View(db.PhoneModels.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneModel phoneModel = db.PhoneModels.Find(id);
            if (phoneModel == null)
            {
                return HttpNotFound();
            }
            return View(phoneModel);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhoneModelId,ModelName")] PhoneModel phoneModel)
        {
            if (ModelState.IsValid)
            {
                db.PhoneModels.Add(phoneModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phoneModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneModel phoneModel = db.PhoneModels.Find(id);
            if (phoneModel == null)
            {
                return HttpNotFound();
            }
            return View(phoneModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhoneModelId,ModelName")] PhoneModel phoneModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phoneModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phoneModel);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneModel phoneModel = db.PhoneModels.Find(id);
            if (phoneModel == null)
            {
                return HttpNotFound();
            }
            return View(phoneModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhoneModel phoneModel = db.PhoneModels.Find(id);
            db.PhoneModels.Remove(phoneModel);
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
