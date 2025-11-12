using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Size
{
    public int Id { get; set; }

    public string SizeName { get; set; } = null!;

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
