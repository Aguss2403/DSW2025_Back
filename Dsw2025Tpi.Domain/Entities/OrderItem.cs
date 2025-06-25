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
    public Product Product { get; set; }
    public OrderItem()
    {
        
    }
    public OrderItem(int quantity,decimal unitPrice)
    {
        UnitPrice = unitPrice;
        Quantity = quantity;
        SubTotal = CalculateSubTotal();
    }

    private decimal CalculateSubTotal()
    {
        return UnitPrice * Quantity;
    }
}
