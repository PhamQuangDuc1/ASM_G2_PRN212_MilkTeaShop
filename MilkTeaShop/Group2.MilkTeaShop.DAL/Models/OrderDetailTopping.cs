using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class OrderDetailTopping
{
    public int Id { get; set; }

    public int OrderDetailId { get; set; }

    public int ToppingId { get; set; }

    public decimal ToppingPrice { get; set; }

    public int Quantity { get; set; }

    public virtual OrderDetail OrderDetail { get; set; } = null!;

    public virtual Product Topping { get; set; } = null!;
}
