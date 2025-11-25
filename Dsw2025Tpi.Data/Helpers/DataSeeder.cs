using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Data;

public static class DataSeeder
{
    public static void Seed(Dsw2025TpiContext context)
    {
        if (context.Roles.Any()) return;

        // Si no existen, los creamos
        var roles = new List<Role>
        {
            new Role
            {
                Name = "user",
            },
            new Role
            {
                Name = "admin",
            }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();
    }
}
