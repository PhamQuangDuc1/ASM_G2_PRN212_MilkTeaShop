using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    public string? PaymentMethod { get; set; }

    public string Status { get; set; } = null!;

    public string OrderSource { get; set; } = null!;

    public string? ExternalOrderId { get; set; }

    public int UserId { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; } = null!;
}
