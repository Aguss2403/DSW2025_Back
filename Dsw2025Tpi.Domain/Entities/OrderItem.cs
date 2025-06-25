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
    public decimal SubTotal { get; set; }
    public Product Product { get; set; }
    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException();
        Quantity = quantity;
        UnitPrice = product.CurrentUnitPrice;
        SubTotal = CalculateSubTotal();
    }

    private decimal CalculateSubTotal()
    {
        return UnitPrice * Quantity;
    }
}
