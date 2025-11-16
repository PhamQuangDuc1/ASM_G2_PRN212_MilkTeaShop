using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class MemberTierServices
    {
        private MemberTierRepo _repo = new();

        public List<MemberTier> GetAllProduct()
        {
            return _repo.GetAll();
        }

        public void UpdateMember(MemberTier x)
        {
            _repo.Update(x);
        }



    }
}
