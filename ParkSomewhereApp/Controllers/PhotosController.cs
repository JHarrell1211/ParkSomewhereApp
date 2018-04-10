using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ParkSomewhereApp.Models;

namespace ParkSomewhereApp.Controllers
{
    public class PhotosController : Controller
    {
        private Models.ParkSomewhereAppEntities db = new Models.ParkSomewhereAppEntities();

        public ActionResult Index()
        {
            ViewBag.ParkID = new SelectList(db.Parks, "ParkID", "ParkName");
            var photos = db.Photos.Include(p => p.Park).Include(p => p.AspNetUser);
            return View(photos.ToList());
        }
        [HttpPost]
        public ActionResult Index(int ParkID)
        {
            ViewBag.ParkID = new SelectList(db.Parks, "ParkID", "ParkName");
            var photos = db.Photos.Include(r => r.Park).Include(r => r.AspNetUser).Where(r => r.ParkID == ParkID).OrderByDescending(r => r.PhotoID);
            return View(photos.ToList());
        }

        [HttpPost]
        public ActionResult Index(int ParkID)
        {
            ViewBag.ParkID = new SelectList(db.Parks, "ParkID", "ParkName");
            var photos = db.Photos.Include(r => r.Park).Include(r => r.AspNetUser).Where(r => r.ParkID == ParkID).OrderByDescending(r => r.PhotoID);
           return View(photos.ToList());
        }


        // GET: Photos
        [HttpGet] [Authorize]
        public ActionResult Add()
        {
            ViewBag.ParkID = new SelectList(db.Parks, "ParkID", "ParkName");
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        [HttpPost]
        public ActionResult Add(Models.Photo imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Image = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            using (Models.ParkSomewhereAppEntities db = new Models.ParkSomewhereAppEntities())
            {
                imageModel.UserID = User.Identity.GetUserId();
                db.Photos.Add(imageModel);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.ParkID = new SelectList(db.Parks, "ParkID", "ParkName", imageModel.ParkID);
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", imageModel.UserID);
            return RedirectToAction("Index", "Photos");


        }

        // GET: Reviews/Delete/5
       public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }







    }
}