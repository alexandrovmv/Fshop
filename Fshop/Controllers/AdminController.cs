using FShop.DB;
using FShop.DB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fshop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
            Console.WriteLine();
        }
      
        public ActionResult Index(int Page=1, int ProdOnPage=3)
        {
            if (!User.IsInRole("AppAdmin"))
            {
                return RedirectToAction("Index", "Product");
            }
            IEnumerable<FShop.DB.DB.Product> products = (repository.Prod.ToList()).Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);
            ViewBag.Page = Page;
            ViewBag.Count = Math.Ceiling((decimal)repository.Prod.Count() / ProdOnPage);
            return View(products);
        }
        public ActionResult Edit(int id)
        {
            Product p = repository.Prod.FirstOrDefault(x => x.ProductID == id);
            if (p == null) p = new Product { ProductID = id };
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit(Product EditedProduct)
        {        
            if (ModelState.IsValid)
            {
                if (EditedProduct.ProductID > repository.Prod.Max(x=>x.ProductID))
                {
                    repository.Prod.Add(EditedProduct);
                    
                }
                else
                {
                    Product p = repository.Prod.FirstOrDefault(x => x.ProductID == EditedProduct.ProductID);
                    p.Price = EditedProduct.Price;
                    p.Name = EditedProduct.Name;
                    p.Photo = EditedProduct.Photo;
                    p.Description = EditedProduct.Description;
                    p.Category = EditedProduct.Category;
                }
                repository.SaveChanges();
                return RedirectToAction("index");
            }
            return View(EditedProduct);
        }
        public ActionResult AddProduct()
        {
            int newId = repository.Prod.Max(x=> x.ProductID) + 1;
            
            return RedirectToAction("Edit",new { id = newId});
        }



        public PartialViewResult GetProductList(int Page = 1, int ProdOnPage = 5)
        {
           
            ViewBag.ProdOnPage = ProdOnPage;
            ViewBag.Page = Page;

            IEnumerable<FShop.DB.DB.Product> products;
            int count = 0;
     
             count = repository.Prod.Count();
             products = repository.Prod.ToList().Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);

            ViewBag.Count = Math.Ceiling((decimal)count / ProdOnPage);


            return PartialView(products);
        }

        //[HttpPost]
        public ActionResult Remove(int? id = null)
        {

            if (ModelState.IsValid)
            {
                Product p = repository.Prod.FirstOrDefault(x => x.ProductID == id);
                repository.Prod.Remove(p);
                repository.SaveChanges();
                return RedirectToAction("Index"/*,new { id = (int)Session["page"]}*/);
            }
            else
                return View();
        }
    }

  
}