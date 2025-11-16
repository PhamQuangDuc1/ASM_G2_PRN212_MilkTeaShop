using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class ProductRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<Product> GetAll()
        {
            _dbContext = new();
            return _dbContext.Products.ToList();
        }
        public void Update(Product x)
        {
            _dbContext = new();
            _dbContext.Products.Update(x);
            _dbContext.SaveChanges();
        }

        public List<Product> SearchProductsByName(string keyword)
        {
            return _dbContext.Products
                .Where(p => p.ProductName.Contains(keyword))
                .ToList();
        }
    }
}
