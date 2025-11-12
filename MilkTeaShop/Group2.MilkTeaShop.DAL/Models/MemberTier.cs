using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class MemberTier
{
    public int Id { get; set; }

    public string TierName { get; set; } = null!;

    public int MinPoints { get; set; }

    public decimal DiscountPercentage { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
