using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class ProductPrice
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
