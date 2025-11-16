using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class PaymentMethodRepo
    {
        private MilkTeaShopDbContext _context;

        public List<PaymentMethod> GetAll()
        {
            _context = new MilkTeaShopDbContext();
            return _context.PaymentMethods.ToList();
        }
    }
}
