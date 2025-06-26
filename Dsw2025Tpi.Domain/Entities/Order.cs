using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order : EntityBase
{
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public string Notes { get; set; }
    public decimal TotalAmount {get; private set; }
    public required Customer Customer { get; set; }
    public required Guid CustomerId { get; set; }
    public required List<OrderItem> OrderItems { get; set; };
    //public required Guid OrderItemID { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    
    public Order()
    { 
    }
    

    public Order(string shippingAddres, string billingAddres, string? notes, List<OrderItem> orderItems, Guid customerId)
    {
        ShippingAddress = shippingAddres;
        BillingAddress = billingAddres;
        Notes = notes;
        OrderDate = DateTime.UtcNow;
        TotalAmount = CalculateTotalAmount();
        OrderItems = orderItems ?? new List<OrderItem>();
        CustomerId = customerId;
    }
    public decimal CalculateTotalAmount() => OrderItems.Sum(item => item.SubTotal);
}
