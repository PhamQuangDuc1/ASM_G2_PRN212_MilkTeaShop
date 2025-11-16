using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;

namespace MilkTeaShop.BLL.Services
{
    public class PaymentServices
    {
        private readonly PaymentRepo _repo = new();

        public List<Payment> GetAll() => _repo.GetAll();

        public List<Payment> GetByOrder(int orderId) => _repo.GetByOrderId(orderId);

        public void AddPayment(int orderId, int paymentMethodId, decimal amount, string? note = null)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                PaymentMethodId = paymentMethodId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                Note = note
            };
            _repo.Add(payment);
        }

        public void UpdatePayment(Payment payment) => _repo.Update(payment);

        public void DeletePayment(int id) => _repo.Delete(id);
    }
}
