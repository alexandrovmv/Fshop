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
        }
      
        public ActionResult Index(int Page=1, int ProdOnPage=3)
        {
            IEnumerable<FShop.DB.DB.Product> products = repository.Products.Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);
            ViewBag.Page = Page;
            ViewBag.Count = Math.Ceiling((decimal)repository.Products.Count() / ProdOnPage);
            return View(products);
        }
        public ActionResult Edit(int id)
        {
            Product p = repository.Products.FirstOrDefault(x => x.ProductID == id);
            return View(p);
        }

        public PartialViewResult GetProductList(int Page = 1, int ProdOnPage = 5)
        {
           
            ViewBag.ProdOnPage = ProdOnPage;
            ViewBag.Page = Page;

            IEnumerable<FShop.DB.DB.Product> products;
            int count = 0;
     
             count = repository.Products.Count();
             products = repository.Products.Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);

            ViewBag.Count = Math.Ceiling((decimal)count / ProdOnPage);


            return PartialView(products);
        }

        //[HttpPost]
        public ActionResult Remove(int? id = null)
        {

            if (ModelState.IsValid)
            {
                Product p = repository.Products.FirstOrDefault(x => x.ProductID == id);
                repository.Products.Remove(p);
                return RedirectToAction("Index"/*,new { id = (int)Session["page"]}*/);
            }
            else
                return View();
        }
    }

  
}