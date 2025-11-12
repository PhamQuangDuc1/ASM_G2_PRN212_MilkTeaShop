using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class Voucher
{
    public int Id { get; set; }

    public string VoucherName { get; set; } = null!;

    public int? VoucherProductId { get; set; }

    public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

    public virtual Product? VoucherProduct { get; set; }
}
