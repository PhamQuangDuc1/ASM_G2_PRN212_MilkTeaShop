using System;
using System.Collections.Generic;

namespace Group2.MilkTeaShop.DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
