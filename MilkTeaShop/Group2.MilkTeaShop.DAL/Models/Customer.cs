using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FullName { get; set; }

    public int? Points { get; set; }

    public DateOnly? Birthday { get; set; }

    public int MemberTierId { get; set; }

    public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

    public virtual MemberTier MemberTier { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
