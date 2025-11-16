using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class Size
{
    public int Id { get; set; }

    public string SizeName { get; set; } = null!;

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
