using FShop.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fshop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        // GET: Product
        public ActionResult Index(string ProdType,int ProdOnPage=3,int Page=1)
        {
            ViewBag.ProdType = ProdType;
            ViewBag.ProdOnPage = ProdOnPage;
            ViewBag.Page = Page;

            IEnumerable<FShop.DB.DB.Product> products;
            int count = 0;
            if (!string.IsNullOrEmpty(ProdType))
            {
                count = repository.Products.Where(x => x.Category == ProdType).Count();
                products = repository.Products.Where(x =>x.Category == ProdType).Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);
            }
            else
            {
                count = repository.Products.Count();
                products = repository.Products.Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);

            }
            ViewBag.Count = Math.Ceiling((decimal)count/ProdOnPage);
            return View(products);
         
        }
        public PartialViewResult Prod_List(string ProdType, int ProdOnPage = 3, int Page = 1)
        {
            IEnumerable<FShop.DB.DB.Product> products;
            int count = 0;
            if (!string.IsNullOrEmpty(ProdType))
            {
                count = repository.Products.Where(x => x.Category == ProdType).Count();
                products = repository.Products.Where(x => x.Category == ProdType).Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);
            }
            else
            {
                count = repository.Products.Count();
                products = repository.Products.Skip((Page - 1) * ProdOnPage).Take(ProdOnPage);

            }
            ViewBag.Count = Math.Ceiling((decimal)count / ProdOnPage);
          

            return PartialView(products);
        }
    }


}