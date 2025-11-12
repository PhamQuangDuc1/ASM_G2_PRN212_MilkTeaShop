using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Category
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
