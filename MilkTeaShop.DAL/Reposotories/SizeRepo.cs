using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class SizeRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<Size> GetAll()
        {
            _dbContext = new();
            return _dbContext.Sizes.ToList();
        }
        public void Update(Size x)
        {
            _dbContext = new();
            _dbContext.Sizes.Update(x);
            _dbContext.SaveChanges();
        }

        
    }
}
