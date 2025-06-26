using Dsw2025Tpi.Domain.Entities;

public class Customer : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // OK: relación 1:N con Order
    public List<Order> Orders { get; set; } = new List<Order>();

    // ❌ Eliminar: no tiene sentido en esta clase
    // public Guid OrderId { get; set; }

    public Customer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
