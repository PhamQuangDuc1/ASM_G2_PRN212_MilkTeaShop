using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ImagePath { get; set; }

    public int CategoryId { get; set; }

    public bool IsActive { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
