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
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            var events = from s in db.Events
                         select s;
            events = events.OrderByDescending(s => s.ID);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Venue,Description,Organizer,Image")] Event @event, HttpPostedFileBase file)
        {
            string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "event.jpg";
            string fileType = fileName.Substring(fileName.LastIndexOf('.'));
            if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") ||(fileType == ".jpeg") || (fileType == ".png")))
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/eventimages/"))+ fileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    string filePathString = path;
                    @event.Image = fileName;
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
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Venue,Description,Organizer,Image")] Event @event, HttpPostedFileBase file)
        {
            string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "event.jpg";
            string fileType = fileName.Substring(fileName.LastIndexOf('.'));
            if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") || (fileType == ".jpeg") || (fileType == ".png")))
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/eventimages/")) + fileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    string filePathString = path;
                    @event.Image = fileName;
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
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Event> Passevent()
        {
            var eveList = db.Events.ToList();
            return eveList;
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
