using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Note { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
