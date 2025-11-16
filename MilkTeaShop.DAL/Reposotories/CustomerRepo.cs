using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class CustomerRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<Customer> GetAll()
        {
            _dbContext = new();
            return _dbContext.Customers.ToList();
        }
        public void Update(Customer x)
        {
            _dbContext = new();
            _dbContext.Customers.Update(x);
            _dbContext.SaveChanges();
        }

        
    }
}
