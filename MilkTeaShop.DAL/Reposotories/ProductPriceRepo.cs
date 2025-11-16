using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class ProductPriceRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<ProductPrice> GetAll()
        {
            _dbContext = new();
            return _dbContext.ProductPrices.ToList();
        }
        public void Update(ProductPrice x)
        {
            _dbContext = new();
            _dbContext.ProductPrices.Update(x);
            _dbContext.SaveChanges();
        }

       
    }
}
