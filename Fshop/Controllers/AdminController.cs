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
            IEnumerable<FShop.DB.DB.Product> products = repository.Products.Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);
            ViewBag.Page = Page;
            ViewBag.Count = Math.Ceiling((decimal)repository.Products.Count() / ProdOnPage);
            return View(products);
        }
        public ActionResult Edit(int id)
        {
            Product p = repository.Products.FirstOrDefault(x => x.ProductID == id);
            if (p == null) p = new Product { ProductID = id };
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit(Product EditedProduct)
        {        
            if (ModelState.IsValid)
            {
                if (EditedProduct.ProductID > repository.Products.Last().ProductID)
                {
                    repository.Products.Add(EditedProduct);
                }
                else
                {
                    Product p = repository.Products.FirstOrDefault(x => x.ProductID == EditedProduct.ProductID);
                    p.Price = EditedProduct.Price;
                    p.Name = EditedProduct.Name;
                    p.Photo = EditedProduct.Photo;
                    p.Description = EditedProduct.Description;
                    p.Category = EditedProduct.Category;
                }
               
                return RedirectToAction("index");
            }
            return View(EditedProduct);
        }
        public ActionResult AddProduct()
        {
            int newId = repository.Products[repository.Products.Count-1].ProductID + 1;
            return RedirectToAction("Edit",new { id = newId});
        }
        //[HttpPost]
        //public ActionResult AddProduct(Product NewProduct)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        Product p = repository.Products.FirstOrDefault(x => x.ProductID == EditedProduct.ProductID);
        //        p.Price = EditedProduct.Price;
        //        p.Name = EditedProduct.Name;
        //        p.Photo = EditedProduct.Photo;
        //        p.Description = EditedProduct.Description;
        //        p.Category = EditedProduct.Category;
        //        return RedirectToAction("index");
        //    }
        //    return View(EditedProduct);
        //}


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