using FShop.DB.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.DB
{
    public interface IProductRepository
    {
        List<Product> Products { get; }
        DbSet<Product> Prod { get; }
        void SaveChanges();
        //int GetCount(string FishingType);
    }
}
