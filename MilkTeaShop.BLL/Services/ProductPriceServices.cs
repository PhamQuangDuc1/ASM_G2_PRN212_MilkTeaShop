using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class ProductPriceServices
    {
        private ProductPriceRepo _repo = new();

        public List<ProductPrice> GetAllProduct()
        {
            return _repo.GetAll();
        }

        public void UpdateAirCon(ProductPrice x)
        {
            _repo.Update(x);
        }

        
    }
}
