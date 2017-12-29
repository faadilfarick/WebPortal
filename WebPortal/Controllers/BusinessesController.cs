using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    public class BusinessesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Businesses
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var businesses = from s in db.Businesses
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                businesses = businesses.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "City":
                    businesses = businesses.OrderBy(s => s.City);
                    break;
                case "category":
                    businesses = businesses.OrderBy(s => s.Category);
                    break;
                default:  // ID descendinf 
                    businesses = businesses.OrderByDescending(s => s.ID);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(businesses.ToPagedList(pageNumber, pageSize));

            //return View(db.Businesses.ToList());
        }

        // GET: Businesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // GET: Businesses/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,City,Address,Category,Description,Image,Latitude,Longitute,Owner")]
        Business business, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Businesses.Add(business);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(business);
        }

        // GET: Businesses/Edit/5
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Title,Address,Category,Description,Latitude,Longitute,Owner")] int? id, Business business)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }
        
        // POST: Businesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,City,Address,Category,Description,Image,Latitude,Longitute,Owner")]
        Business business, HttpPostedFileBase file)
        {
            string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "business.jpg";
            string fileType = fileName.Substring(fileName.LastIndexOf('.'));
            if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") || (fileType == ".jpeg") || (fileType == ".png")))
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/businessimages/")) + fileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    string filePathString = path;
                    business.Image = fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: File is Not Selected or is not an image. Upload only \".jpg\" \".jpeg\" or \".png\" file types" + ex.Message.ToString();
                }

            }
            else
            {
                ViewBag.Message = "You have not specified a file. ";
            }

            if (ModelState.IsValid)
            {
                db.Entry(business).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(business);
        }

        // GET: Businesses/Delete/5
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Business business = db.Businesses.Find(id);
            db.Businesses.Remove(business);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Business> Passbusiness()
        {
            var business = from s in db.Businesses
                         select s;
            business = business.OrderByDescending(s => s.ID);
            return business.ToList();
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
