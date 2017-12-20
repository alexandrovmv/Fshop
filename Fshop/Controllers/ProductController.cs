using Fshop.Models;
using FShop.DB;
using FShop.DB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fshop.Controllers
{
    public class ProductController : Controller
    {
        static int UId { get; set; }
        private IProductRepository repository;
        public static List<Cart> Carts { get; set; }
        public Cart CurrentCart { get; set; }
        public ProductController(IProductRepository repo)
        {
            repository = repo;
            if (Carts == null) Carts = new List<Cart>();
        }
        // GET: Product
        public ActionResult Index(string ProdType,int ProdOnPage=3,int Page=1)
        {
            object UserId = Session["UId"];
            if ( !Session.IsNewSession)
            {
                CurrentCart = Carts.FirstOrDefault(x => x.UId == Session.LCID);
            }
            else {
                CurrentCart = new Cart { UId = Session.LCID };
                Carts.Add(CurrentCart);                
                Session["UId"] = Session.LCID;
                UId++;
            }
            ViewBag.ProdType = ProdType;
            ViewBag.ProdOnPage = ProdOnPage;
            ViewBag.Page = Page;
            ViewBag.UId = CurrentCart.UId;
            ViewBag.ProductCount = CurrentCart.Check.Count;
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
            if (Math.Ceiling((decimal)count / ProdOnPage) < Page) ViewBag.Page = Math.Ceiling((decimal)count / ProdOnPage) < Page;

            return View(products);
         
        }

        public ActionResult Map()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("1", "Автомобиль");
            dict.Add("2", "Веломашина");
            dict.Add("3", "на общественном транспорте");
            dict.Add("4", "Пешком");

            ViewBag.TravelMode = new SelectList(dict, "Key", "Value");
            return View();
        }
        public ActionResult Contats()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("1", "Автомобиль");
            dict.Add("2", "Веломашина");
            dict.Add("3", "на общественном транспорте");
            dict.Add("4", "Пешком");

            ViewBag.TravelMode = new SelectList(dict, "Key", "Value");

            return View();
        }
        public JsonResult GetData()
        {
            // создадим список данных
            List<Shop> stations = new List<Shop>();
            stations.Add(new Shop()
            {
                Id = 1,
                PlaceName = "Магазин Флагман",
                GeoLat = 37.610489,
                GeoLong = 55.752308,
              
            });
        

            return Json(stations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Bucket(int id)
        {
            CurrentCart = Carts.FirstOrDefault(x=>x.UId==id);
            return View(CurrentCart);
        }
        public ActionResult AddProduct(int prodId)
        {
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            if (prodId != 0)
            {
              
                CartItem CI = CurrentCart.Check.FirstOrDefault(x => x.product.ProductID == prodId);
                if (CI == null)
                {
                    CurrentCart.Check.Add(
                        new CartItem
                        {
                            product = repository.Products.FirstOrDefault(x => x.ProductID == prodId),
                            Count = 1
                        });
                }
                else
                    CI.Count++;
            }
           
            ViewBag.ProductCount = CurrentCart.Check.Count;
            return PartialView();
        }

        public PartialViewResult GetPoductCount() {
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            if (CurrentCart != null)
                ViewBag.ProductCount = CurrentCart.Check.Count;
            else
                ViewBag.ProductCount = 0;
            return PartialView();
        }

        public PartialViewResult Prod_List(string ProdType, int ProdOnPage = 3, int Page = 1)
        {
            ViewBag.ProdType = ProdType;
            ViewBag.ProdOnPage = ProdOnPage;
            ViewBag.Page = Page;

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

        //[HttpPost]
        public ActionResult Remove(int? id = null)
        {
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            if (ModelState.IsValid)
            {
                CartItem CI = CurrentCart.Check.FirstOrDefault(x => x.product.ProductID == id);
                CurrentCart.Check.Remove(CI);
                return RedirectToAction("Bucket",new { id = (int)Session["UId"] });
            }
            else
                return View();
        }
        public ActionResult ClearBucket(){
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            CurrentCart.Check.Clear();
            return RedirectToAction("Bucket", new { id = (int)Session["UId"] });
        }

        public ActionResult Edit(int id)
        {
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            CartItem CI = CurrentCart.Check.FirstOrDefault(x => x.product.ProductID == id);
         
            return View(CI);
        }
        [HttpPost]
        public ActionResult Edit(CartItem EditedCartItem)
        {
            CurrentCart = Carts.FirstOrDefault(x => x.UId == Convert.ToInt32(Session["UId"]));
            if (ModelState.IsValid)
            {
           
                    CartItem CI = CurrentCart.Check.FirstOrDefault(x => x.product.ProductID == EditedCartItem.product.ProductID);
                CI.Count = EditedCartItem.Count;
                

                return RedirectToAction("index");
            }
            return View(EditedCartItem);
        }
    }


}