using Microsoft.AspNet.Identity;
using ProiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDaw.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Review
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
           
            return View();
        }
        [NonAction]
        public void New(Review rev)
        {

           
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    TempData["message"] = "Comentariu a fost adaugat!";
                    
                }
              
            
        }
        public ActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);
            db.Reviews.Remove(rev);
            TempData["message"] = "Comentariul a fost stears!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}