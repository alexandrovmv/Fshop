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
        public IEnumerable<Product> Products
        {
            get
            {
                return context.Product;
            }
        }
    }
}
