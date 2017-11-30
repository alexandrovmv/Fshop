using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FShop.DB.DB;

namespace FShop.DB
{
    public class ProductRepository : IProductRepository
    {
        FShopContext context = new FShopContext();

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

        
    }
}
