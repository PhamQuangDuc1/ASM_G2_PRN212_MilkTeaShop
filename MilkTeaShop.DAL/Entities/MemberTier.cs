using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class MemberTier
{
    public int Id { get; set; }

    public string TierName { get; set; } = null!;

    public int MinPoints { get; set; }

    public decimal DiscountPercentage { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
