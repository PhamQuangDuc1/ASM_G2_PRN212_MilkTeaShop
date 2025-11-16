using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class CustomerServices
    {
        private CustomerRepo _repo = new();

        public List<Customer> GetAllCus()
        {
            return _repo.GetAll();
        }

    }
}
