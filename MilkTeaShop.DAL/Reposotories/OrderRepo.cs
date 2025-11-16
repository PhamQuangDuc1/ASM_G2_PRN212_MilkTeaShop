using Microsoft.EntityFrameworkCore;
using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class OrderRepo
    {
        private MilkTeaShopDbContext db;
        public List<Order> GetOrdersByTable(int tableId)
        {
             db = new MilkTeaShopDbContext();
            return db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.OrderDetailToppings)
                        .ThenInclude(odt => odt.Topping)
                .Where(o => o.TableId == tableId && o.Status == "Active")
                .ToList();
        }

        public void CreateOrder(Order order)
        {
             db = new MilkTeaShopDbContext();
            db.Add(order);
            db.SaveChanges();
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            db = new MilkTeaShopDbContext();
            db.OrderDetails.Add(detail);
            db.SaveChanges();
        }

    }
}
