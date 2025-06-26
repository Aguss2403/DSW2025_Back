using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Api.Utils;

public static class DomainServicesConfigurationExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Dsw2025TpiEntities"));
            options.UseSeeding((c, t) =>
            {
                ((Dsw2025TpiContext)c).SeedWork<Customer>("Sources\\customer.json");
                //((Dsw2025TpiContext)c).SeedWork<Product>("Sources\\product.json");
                //((Dsw2025TpiContext)c).SeedWork<Order>("Sources\\order.json");
                //((Dsw2025TpiContext)c).SeedWork<OrderItem>("Sources\\orderitem.json");
            });
        });
        return services;

    }
}
