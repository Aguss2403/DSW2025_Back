using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class User : EntityBase
{

    public User() { }
    public User(string username, string email, string password, Role role)
    {
        Username = username;
        Email = email;
        Password = password;
        Role = role;
        RoleId = role.Id;
    }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Customer? Customer { get; set; }
}
