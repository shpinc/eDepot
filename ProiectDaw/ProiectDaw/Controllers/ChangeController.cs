using ProiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDaw.Controllers
{
    public class ChangeController : Controller
    {
        static private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Change
        public ActionResult Index()
        {
            return View();
        }
        
        static public void NewC(String OldV, String NewV, String FieldC, DateTime DT) 
        {
            Change change = new Change();
            change.OldValue = OldV;
            change.NewValue = NewV;
            change.FieldChanged = FieldC;
            change.DateChanged = DT;
            try
            {
                
                    db.Changes.Add(change);
                    db.SaveChanges();
                    
                
            }
            catch (Exception e)
            {
                
            }

        }
        
    }
}