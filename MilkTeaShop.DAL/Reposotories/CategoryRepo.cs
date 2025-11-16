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

        public Category Add(Category category)
        {
            _dbContext = new();
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category;
        }

        public bool Delete(int id)
        {
            _dbContext = new();
            var category = _dbContext.Categories.Find(id);
            if (category == null) return false;
            
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Category> GetAll()
        {
            _dbContext = new();
            return _dbContext.Categories.ToList();
        }

        public Category? GetById(int id)
        {
            _dbContext = new();
            return _dbContext.Categories.Find(id);
        }

        public Category Update(Category category)
        {
            _dbContext = new();
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            return category;
        }
    }
}
