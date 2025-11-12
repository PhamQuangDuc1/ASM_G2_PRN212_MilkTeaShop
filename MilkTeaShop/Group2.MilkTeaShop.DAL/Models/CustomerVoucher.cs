using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class CustomerVoucher
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int VoucherId { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly? ExpiryDate { get; set; }

    public int? UsedOrderId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order? UsedOrder { get; set; }

    public virtual Voucher Voucher { get; set; } = null!;
}
