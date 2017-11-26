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
            if (!string.IsNullOrEmpty(ProdType))
                return View(repository.Products.Where(x => { return x.Category == ProdType; }).Skip((Page - 1) * ProdOnPage).Take(ProdOnPage));

            return View(repository.Products.Skip((Page-1)*ProdOnPage).Take(ProdOnPage));
        }
    }


}