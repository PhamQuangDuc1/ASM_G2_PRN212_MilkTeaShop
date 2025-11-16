using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilkTeaShop.DAL.Reposotories
{
    public class PaymentRepo
    {
        public List<Payment> GetAll()
        {
            using var db = new MilkTeaShopDbContext();
            return db.Payments
                     .OrderByDescending(p => p.PaymentDate)
                     .ToList();
        }

        public List<Payment> GetByOrderId(int orderId)
        {
            using var db = new MilkTeaShopDbContext();
            return db.Payments
                     .Where(p => p.OrderId == orderId)
                     .OrderByDescending(p => p.PaymentDate)
                     .ToList();
        }

        public Payment? GetById(int id)
        {
            using var db = new MilkTeaShopDbContext();
            return db.Payments.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Payment payment)
        {
            using var db = new MilkTeaShopDbContext();
            db.Payments.Add(payment);
            db.SaveChanges();
        }

        public void Update(Payment payment)
        {
            using var db = new MilkTeaShopDbContext();
            db.Payments.Update(payment);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            using var db = new MilkTeaShopDbContext();
            var p = db.Payments.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                db.Payments.Remove(p);
                db.SaveChanges();
            }
        }
    }
}
