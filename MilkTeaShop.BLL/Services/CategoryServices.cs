using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class CategoryServices
    {
        private CategoryRepo _repo = new();

        public List<Category> GetAllServices()
        {
            return _repo.GetAll();
        }
    }
}
