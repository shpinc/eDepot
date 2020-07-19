using Microsoft.AspNet.Identity;
using ProiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDaw.Controllers
{
    
    public class ProductController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext(); 
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var Products = from Product in db.Products
                           where Product.IsApproved == true
                           orderby Product.Title
                           select Product;
            ViewBag.Products = Products;

            var Reviews = from Review in db.Reviews
                          select Review;
            ViewBag.Reviews = Reviews;
                     
            return View();
        }

        public ActionResult SortA()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var Products = from Product in db.Products
                           where Product.IsApproved == true
                           orderby Product.Price
                           select Product;
            ViewBag.Products = Products;

            var Reviews = from Review in db.Reviews
                          select Review;
            ViewBag.Reviews = Reviews;

            return View();
        }
        public ActionResult Search(String name) 
        {

            return RedirectToAction("Index");
         int id = (from Product in db.Products
                  where Product.Title == name
                  select Product).First().Id;
            return RedirectToAction("/Show/"+id);

        }

        public ActionResult SortD()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var Products = from Product in db.Products
                           where Product.IsApproved == true
                           orderby Product.Price descending
                           select Product;
            ViewBag.Products = Products;

            var Reviews = from Review in db.Reviews
                          select Review;
            ViewBag.Reviews = Reviews;

            return View();
        }
        /*GET: ALl from category with given id
        public ActionResult Index(int id)
        {
            

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var products = db.Products.Include("Category").Include("User");
            Category category = db.Categories.Find(id);
            var Products = from Product in db.Products
                           where (Product.Category.CategoryId == id)&&(Product.IsApproved == true)
                           orderby Product.Title
                           select Product;

            ViewBag.Products = Products;
            return View();
        }
        */
        [Authorize(Roles = "Admin")]
        public ActionResult PendingApproval()
        {
            var Products = from Product in db.Products
                           where Product.IsApproved == false
                           orderby Product.Title
                           select Product;
            ViewBag.Products = Products;
            return View();
        }
        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            var Reviews = from Review in db.Reviews
                          where Review.ProductId==id
                          select Review;
            ViewBag.Reviews = Reviews;
            return View(product);

        }
        [Authorize(Roles = "Admin,Collaborator")]
        public ActionResult New()
        {
            Product product = new Product();
            product.Categories = GetAllCategories();//does it make snese?(linia 71)
            return View(product);
        }
        [Authorize(Roles = "Admin,Collaborator")]
        [HttpPost]
        public ActionResult New(Product product)
        {
            product.Categories = GetAllCategories();//does it make sense?(linia 61)
            product.UserId = User.Identity.GetUserId();
            product.ReviewsId = null; ;
            //product.ChangesId = null;
            product.IsApproved = false;
            product.Rating = 0;
            product.LastDateUpdated = DateTime.Today;
            product.Image_path = "DE STERS";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["message"] = "Produsul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Datele introduse nu respecta formatul!";
                    return View(product);
                }
            }
            catch (Exception e)
            {
                return View(product);
            }
        }
        [Authorize(Roles = "Admin,Collaborator")]
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                ViewBag.Product = product;
                product.Categories = GetAllCategories();
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Collaborator")]
        public ActionResult Edit(int id, Product requestProduct)
        {
            int i = 0;
            requestProduct.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);
                    if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        //if (TryUpdateModel(product))
                        //{
                            if(product.Title!=requestProduct.Title)
                            {   
                                i++;
                            ChangeController.NewC(product.Title, requestProduct.Title, "Title", DateTime.Now);
                                product.Title = requestProduct.Title;
                            }
                            if (product.Description != requestProduct.Description)
                            {
                                i++;
                                ChangeController.NewC(product.Description, requestProduct.Description, "Title", DateTime.Now);
                                product.Description = requestProduct.Description;
                            }
                            if (product.Price != requestProduct.Price)
                            {
                                i++;
                                ChangeController.NewC(product.Price.ToString(), requestProduct.Price.ToString(), "Pret", DateTime.Now);
                                product.Price = requestProduct.Price;
                            }
                            if (product.Image_path != requestProduct.Image_path)
                            {
                                i++;
                                ChangeController.NewC(product.Image_path, requestProduct.Image_path, "Poza", DateTime.Now);
                                product.Image_path = requestProduct.Image_path;
                            }
                            if (product.CategoryId != requestProduct.CategoryId)
                            {   
                                i++;
                                ChangeController.NewC(product.CategoryId.ToString(), requestProduct.CategoryId.ToString(), "Categorie", DateTime.Now);
                                product.CategoryId = requestProduct.CategoryId;
                            }
                            product.LastDateUpdated = DateTime.Today;
                            db.SaveChanges();
                            TempData["message"] = TempData["message"]+"Produsul a fost modificat! Au fost modificate" + i.ToString()+ "campuri.";
                            return RedirectToAction("Index");
                        //}
                        //else 
                        //{
                            //TempData["message"] = "Else in try updatee error";
                           // return RedirectToAction("Index");
                        //}
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestProduct);
                }

            }
            catch (Exception e)
            {
                return View(requestProduct);
            }
        }

        [Authorize(Roles = "Admin,Collaborator")]
        [HttpDelete]
        
        public ActionResult Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(id);
                    if (product.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        TempData["message"] = "Articolul a fost sters!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["message"] = "Eroare la conectarea la baza de date, produsul nu a fost sters!";
                    return RedirectToAction("Index"); 
                }
            }
            catch(Exception e)
            {
                TempData["message"] = "Exceptie, produsul nu a fost sters!";
                return RedirectToAction("Index");
            }
 }
        [NonAction]
        public float Compute_rating(int ProductId)
        {
            var reviews = from rev in db.Reviews
                          where rev.ProductId == ProductId
                          select rev;
            var review_number = (from rev in db.Reviews
                                 where rev.ProductId==ProductId
                                  select rev).Count();
            float sum = 0;
            foreach (Review Rev in reviews)
            {
                sum = Rev.rating + sum;
            }
            return sum / review_number;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }
        
        public ActionResult ApproveProduct(int id)
        {
            Product product = (from Product in db.Products
                              where Product.IsApproved == false
                              orderby Product.Title
                              select Product).FirstOrDefault();
            product.IsApproved = true;
            try
            {
                db.SaveChanges();
                TempData["message"] = "Produsul a fost aprobat!";
                return RedirectToAction("PendingApproval");
            }
            catch (Exception e)
            {
                TempData["message"] = "Exceptie";
                return RedirectToAction("Index");


            }

        
        }

        
        [NonAction]
        [Authorize(Roles = "Admin,Collaborator,Registred User") ]
        public void AdaugaInCos(int id)
        {

            string UserId = User.Identity.GetUserId();

             ApplicationUser current = (from usr in db.Users
                        where usr.Id == UserId
                        select usr ).FirstOrDefault();
            current.CosCumparaturi.Add(id);
            

        }
    }
}