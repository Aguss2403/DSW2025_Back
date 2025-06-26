using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class OrderItem : EntityBase
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; private set; }
    public required Product Product { get; set; }
    public required Guid ProductId { get; set; }
    public OrderItem()
    {
        
    }
    public OrderItem(int quantity, Guid productID)
    {
        UnitPrice = Product!.CurrentUnitPrice;
        Quantity = quantity;
        SubTotal = CalculateSubTotal();
        ProductId = productID;
    }

    private decimal CalculateSubTotal()
    {
        return UnitPrice * Quantity;
    }
}
