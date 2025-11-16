using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public  class OrderServices
    {
        private readonly OrderRepo _repo = new();

        public List<Order> GetOrdersByTable(int tableId)
        {
            return _repo.GetOrdersByTable(tableId);
        }

        public void CreateOrder(Order order)
        {
            _repo.CreateOrder(order);
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            _repo.AddOrderDetail(detail);

        }


    }
}
