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
    public Customer Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public Order()
    {
        
    }
    public Order(string shippingAddres, string billingAddres, string notes)
    {
        ShippingAddress = shippingAddres;
        BillingAddress = billingAddres;
        Notes = notes;
        OrderDate = DateTime.UtcNow;
        TotalAmount = CalculateTotalAmount();
    }
    public decimal CalculateTotalAmount() => OrderItems.Sum(item => item.SubTotal);
}
