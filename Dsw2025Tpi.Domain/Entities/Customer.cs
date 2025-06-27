using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Customer : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    //public List<Order> Orders { get; set; } = new List<Order>();
    public Guid OrderId { get; set; }

    public Customer(string name, string email, string phoneNumber, Guid orderId)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        OrderId = orderId;
    }
}
