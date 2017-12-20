using FShop.DB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.DB
{
    public interface IProductRepository
    {
        List<Product> Products { get; }
        //int GetCount(string FishingType);
    }
}
