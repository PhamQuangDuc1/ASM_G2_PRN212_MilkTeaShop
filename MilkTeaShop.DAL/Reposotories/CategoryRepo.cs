using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class CategoryRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<Category> GetAll()
        {
            _dbContext = new();
            return _dbContext.Categories.ToList();
        }
    }
}
