using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using System.Web.Mvc;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = from s in db.Movies
                         select s;
            movies = movies.OrderByDescending(s => s.ID);
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Category,Description,Producer,Image")] Movie movie, HttpPostedFileBase file)
        {
            string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "movie.jpg";
            string fileType = fileName.Substring(fileName.LastIndexOf('.'));
            if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") ||(fileType == ".jpeg") || (fileType == ".png")) )
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/movieimages/"))+ fileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    string filePathString = path;
                    movie.Image = fileName;
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
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Category,Description,Producer,Image")] Movie movie, HttpPostedFileBase file)
        {
            string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "movie.jpg";
            string fileType = fileName.Substring(fileName.LastIndexOf('.'));
            if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") || (fileType == ".jpeg") || (fileType == ".png")))
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/movieimages/")) + fileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    string filePathString = path;
                    movie.Image = fileName;
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
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }       

        // GET: Movies/Delete/5
        [Authorize(Users = "admin@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Movie> Passmovie()
        {
            var movies = from s in db.Movies
                         select s;
            movies = movies.OrderByDescending(s => s.ID);
            return movies.ToList();
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
