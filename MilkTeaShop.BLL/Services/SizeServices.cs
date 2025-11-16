using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class SizeServices
    {
        private SizeRepo _repo = new();

        public List<Size> GetAllProduct()
        {
            return _repo.GetAll();
        }

        public void UpdateSize(Size x)
        {
            _repo.Update(x);
        }

        
    }
}
