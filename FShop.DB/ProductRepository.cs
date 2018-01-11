using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FShop.DB.DB;
using System.Data.Entity;

namespace FShop.DB
{
    public class ProductRepository : IProductRepository
    {
        static FShopContext context = new FShopContext();
        static DbSet<Product> Prods { get; set; }
        public DbSet<Product> Prod { get { return context.Product; } }
        static List<Product> prod;
        public ProductRepository()
        {
           
            if(prod==null)
                prod = context.Product.ToList();
        }
        public List<Product> Products
        {
            get
            {
                return prod;
            }
        }

        public void SaveChanges()
        {

            context.SaveChanges();
            
        }
    }
}
