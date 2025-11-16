using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string? SugarLevel { get; set; }

    public string? IceLevel { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Notes { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();

    public virtual Product Product { get; set; } = null!;
}
