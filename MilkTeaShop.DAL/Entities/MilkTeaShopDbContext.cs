using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;

namespace MilkTeaShop.DAL;

public partial class MilkTeaShopDbContext : DbContext
{
    public MilkTeaShopDbContext()
    {
    }

    public MilkTeaShopDbContext(DbContextOptions<MilkTeaShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerVoucher> CustomerVouchers { get; set; }

    public virtual DbSet<MemberTier> MemberTiers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailTopping> OrderDetailToppings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

         => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC072E3FB201");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07F076D4B5");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Customer__85FB4E3876405FB1").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.MemberTierId).HasDefaultValue(1);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Points).HasDefaultValue(0);

            entity.HasOne(d => d.MemberTier).WithMany(p => p.Customers)
                .HasForeignKey(d => d.MemberTierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__Member__5535A963");
        });

        modelBuilder.Entity<CustomerVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07ACF85C89");

            entity.ToTable("CustomerVoucher");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Chưa dùng");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVouchers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Custo__5AEE82B9");

            entity.HasOne(d => d.UsedOrder).WithMany(p => p.CustomerVouchers)
                .HasForeignKey(d => d.UsedOrderId)
                .HasConstraintName("FK_CustomerVoucher_Order");

            entity.HasOne(d => d.Voucher).WithMany(p => p.CustomerVouchers)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Vouch__5BE2A6F2");
        });

        modelBuilder.Entity<MemberTier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MemberTi__3214EC07298FC3F7");

            entity.ToTable("MemberTier");

            entity.HasIndex(e => e.TierName, "UQ__MemberTi__DB82ACA1B046DB0C").IsUnique();

            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TierName).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07C64E630A");

            entity.ToTable("Order");

            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExternalOrderId).HasMaxLength(100);
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderSource)
                .HasMaxLength(50)
                .HasDefaultValue("Tại quầy");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CreatedBy__628FA481");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__CustomerI__6383C8BA");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK_Order_Tables");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07D610E8ED");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.IceLevel).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.SugarLevel).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__66603565");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__6754599E");
        });

        modelBuilder.Entity<OrderDetailTopping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07D6B5249A");

            entity.ToTable("OrderDetailTopping");

            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.ToppingPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.OrderDetailToppings)
                .HasForeignKey(d => d.OrderDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__6B24EA82");

            entity.HasOne(d => d.Topping).WithMany(p => p.OrderDetailToppings)
                .HasForeignKey(d => d.ToppingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Toppi__6C190EBB");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07B0503CC2");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__OrderId__73BA3083");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Payment__74AE54BC");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07ECD22269");

            entity.ToTable("PaymentMethod");

            entity.HasIndex(e => e.MethodName, "UQ__PaymentM__218CFB17D3BED0AD").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MethodName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC0776958123");

            entity.ToTable("Product");

            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductName).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__412EB0B6");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC07AFDBF532");

            entity.ToTable("ProductPrice");

            entity.HasIndex(e => new { e.ProductId, e.SizeId }, "UQ_Product_Size").IsUnique();

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__Produ__49C3F6B7");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__SizeI__4AB81AF0");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07B6E1D045");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "UQ__Role__8A2B6160E6946756").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Size__3214EC07D1633A6F");

            entity.ToTable("Size");

            entity.HasIndex(e => e.SizeName, "UQ__Size__619EFC3EA8426CD3").IsUnique();

            entity.Property(e => e.SizeName).HasMaxLength(20);
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Tables__7D5F018E0E6BD932");

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Sẵn sàng");
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC077DDD5C20");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E497BC2F54").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MustChangePassword).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__3A81B327");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher__3214EC0701D16AE5");

            entity.ToTable("Voucher");

            entity.Property(e => e.VoucherName).HasMaxLength(100);

            entity.HasOne(d => d.VoucherProduct).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.VoucherProductId)
                .HasConstraintName("FK__Voucher__Voucher__5812160E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
