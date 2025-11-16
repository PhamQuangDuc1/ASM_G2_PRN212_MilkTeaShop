using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class MemberTierRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public List<MemberTier> GetAll()
        {
            _dbContext = new();
            return _dbContext.MemberTiers.ToList();
        }
        public void Update(MemberTier x)
        {
            _dbContext = new();
            _dbContext.MemberTiers.Update(x);
            _dbContext.SaveChanges();
        }

       
    }
}
