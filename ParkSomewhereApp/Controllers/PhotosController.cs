using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ParkSomewhereApp.Controllers
{
    public class PhotosController : Controller
    {
        private Models.ParkSomewhereAppEntities db = new Models.ParkSomewhereAppEntities();


        public ActionResult Index()
        {
            var photos = db.Photos.Include(p => p.Park).Include(p => p.AspNetUser);
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
            return View();


        }




    }
}