using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer
        modelBuilder.Entity<Customer>(eb =>
        {
            eb.ToTable("Customers");
            eb.HasKey(c => c.Id);
            eb.Property(c => c.Name).HasMaxLength(50).IsRequired();
            eb.Property(c => c.Email).HasMaxLength(100).IsRequired();
            eb.Property(c => c.PhoneNumber).HasMaxLength(15).IsRequired();

            // Relación: Customer tiene muchas Orders
            eb.HasMany(c => c.Orders)
              .WithOne(o => o.Customer)
              .HasForeignKey(o => o.CustomerId)
              .OnDelete(DeleteBehavior.Cascade);
        });

        // Product
        modelBuilder.Entity<Product>(eb =>
        {
            eb.ToTable("Products");
            eb.HasKey(p => p.Id);
            eb.Property(p => p.Sku).HasMaxLength(20).IsRequired();
            eb.Property(p => p.Name).HasMaxLength(50).IsRequired();
            eb.Property(p => p.Description).HasMaxLength(250);
            eb.Property(p => p.CurrentUnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            eb.Property(p => p.StockQuantity).IsRequired();
            eb.Property(p => p.IsActive).IsRequired();
        });

        // Order
        modelBuilder.Entity<Order>(eb =>
        {
            eb.ToTable("Orders");
            eb.HasKey(o => o.Id);
            eb.Property(o => o.OrderDate).IsRequired();
            eb.Property(o => o.ShippingAddress).HasMaxLength(150).IsRequired();
            eb.Property(o => o.BillingAddress).HasMaxLength(150).IsRequired();
            eb.Property(o => o.Notes).HasMaxLength(500);
            eb.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();

            // Relación: Order tiene muchas OrderItems
            eb.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.ToTable("OrderItems");
            eb.HasKey(oi => oi.Id);
            eb.Property(oi => oi.Quantity).IsRequired();
            eb.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            eb.Property(oi => oi.SubTotal).HasColumnType("decimal(18,2)").IsRequired();

            // FK con Product
            eb.HasOne(oi => oi.Product)
              .WithMany() // no navegación inversa
              .HasForeignKey(oi => oi.ProductId)
              .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
