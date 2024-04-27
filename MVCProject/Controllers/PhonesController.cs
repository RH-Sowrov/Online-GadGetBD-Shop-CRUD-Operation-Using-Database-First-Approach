using MVCProject.Models;
using MVCProject.Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    [Authorize]
    public class PhonesController : Controller
    {
      private readonly GadgetDBContext db=new GadgetDBContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult PhoneInfo(int page=1)
        {
            var data = db.Phones.Include(s => s.StockDetails).Include(s => s.PhoneModel).Include(s => s.Brand).OrderBy(s => s.PhoneId).ToPagedList(page, 5);
            return PartialView("_phoneInfo",data);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            PhoneVM model = new PhoneVM();
            model.StockDetails.Add(new StockDetail());
            ViewBag.PhoneModel = db.PhoneModels.ToList();
            ViewBag.BrandModel = db.Brands.ToList();
            return PartialView("_CreateForm", model);
        }
        [HttpPost]
        public ActionResult Create(PhoneVM model , string cct="")
        {
            if (cct == "add")
            {
                model.StockDetails.Add(new StockDetail());
                foreach (var e in ModelState.Values) 
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (cct.StartsWith("remove"))
            {
                int index = int.Parse(cct.Substring(cct.IndexOf("_") + 1));
                model.StockDetails.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if(cct== "insert")
            {
                if(ModelState.IsValid)
                {
                    var phone = new Phone
                    {
                        BrandId = model.BrandId,
                        PhoneModelId = model.PhoneModelId,
                        PhoneName = model.PhoneName,
                        ReliseDate = model.ReliseDate,
                        IsOfficial = model.IsOfficial
                    };
                    //For Picture & File Path
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string ext2 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Picture"), ext2);
                    model.Picture.SaveAs(savePath);
                    phone.Picture = ext2;
                    db.Phones.Add(phone);
                    db.SaveChanges();

                    foreach (var s in model.StockDetails) 
                    {
                        db.Database.ExecuteSqlCommand($"spInsertStock {(int)s.Color},{s.Price},{(int)s.Quantity},{phone.PhoneId}");
                    }
                    PhoneVM model2 = new PhoneVM()
                    {
                        PhoneName = "",
                        ReliseDate = DateTime.Today
                    };
                    model2.StockDetails.Add(new StockDetail());
                    ViewBag.PhoneModel = db.PhoneModels.ToList();
                    ViewBag.BrandModel=db.Brands.ToList();
                    foreach (var e in ModelState.Values)
                    {
                        e.Value = null;
                    }
                    return View("_CreateForm", model2);
                }
            }
            ViewBag.PhoneModel = db.PhoneModels.ToList();
            ViewBag.BrandModel = db.Brands.ToList();
            return View("_CreateForm", model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data = db.Phones.FirstOrDefault(x => x.PhoneId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.StockDetails).Load();
            PhoneEditVM model = new PhoneEditVM()
            {
                PhoneId = id,
                BrandId = data.BrandId,
                PhoneModelId = data.PhoneModelId,
                PhoneName = data.PhoneName,
                ReliseDate = data.ReliseDate,
                IsOfficial = data.IsOfficial,
                StockDetails = data.StockDetails.ToList()
            };
            ViewBag.PhoneModel = db.PhoneModels.ToList();
            ViewBag.BrandModel = db.Brands.ToList();
            ViewBag.CurrentPic = data.Picture;
            return PartialView("_editForm", model);
        }
        [HttpPost]
        public ActionResult Edit(PhoneEditVM mo, string cct = "")
        {
            if (cct == "add")
            {
                mo.StockDetails.Add(new StockDetail());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (cct.StartsWith("remove"))
            {
                int index = int.Parse(cct.Substring(cct.IndexOf("_") + 1));
                mo.StockDetails.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (cct == "update")
            {
                if (ModelState.IsValid)
                {
                    var phone = db.Phones.FirstOrDefault(x => x.PhoneId == mo.PhoneId);
                    if (phone == null) { return new HttpNotFoundResult(); }
                    phone.PhoneName = mo.PhoneName;
                    phone.ReliseDate = mo.ReliseDate;
                    phone.IsOfficial = mo.IsOfficial;
                    phone.BrandId = mo.BrandId;
                    phone.PhoneModelId = mo.PhoneModelId;
                    if (mo.Picture != null)
                    {
                        string ext = Path.GetExtension(mo.Picture.FileName);
                        string ext2 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Picture"), ext2);
                        mo.Picture.SaveAs(savePath);
                        phone.Picture = ext2;
                    }
                    else
                    {

                    }

                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC spDeleteStock {phone.PhoneId}");
                    foreach (var s in mo.StockDetails)
                    {
                        db.Database.ExecuteSqlCommand($"EXEC spInsertStock {(int)s.Color}, {s.Price}, {s.Quantity}, {phone.PhoneId}");
                    }
                }
            }
            ViewBag.PhoneModel = db.PhoneModels.ToList();
            ViewBag.BrandModel = db.Brands.ToList();
            ViewBag.CurrentPic = db.Phones.FirstOrDefault(x => x.PhoneId == mo.PhoneId)?.Picture;
            return View("_EditForm", mo);
        }
        public ActionResult Delete(int? id)
        {
            var phones = db.Phones.FirstOrDefault(x => x.PhoneId == id);
            if (phones != null)
            {
                var phnmdl = db.StockDetails.Where(x => x.PhoneId == id).ToList();
                db.StockDetails.RemoveRange(phnmdl);
                db.Phones.Remove(phones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        
    }
}