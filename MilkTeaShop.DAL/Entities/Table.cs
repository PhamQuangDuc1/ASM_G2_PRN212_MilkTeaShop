using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class Table
{
    public int TableId { get; set; }

    public string TableName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
