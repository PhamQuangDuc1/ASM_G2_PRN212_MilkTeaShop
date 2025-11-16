using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class ProductServices
    {
        private ProductRepo _repo = new();

        public List<Product> GetAllProduct()
        {
            return _repo.GetAll();
        }

        public void UpdateProduct(Product x)
        {
            _repo.Update(x);
        }

        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repo.GetAll();
            return _repo.SearchProductsByName(keyword);
        }
        public List<Product> GetToppingList()
        {
            return _repo.GetAll()
                        .Where(p => p.CategoryId == 2)
                        .ToList();
        }
    }
}
