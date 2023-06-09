using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DbContextServiceExtension
{
    public static IServiceCollection AddDbContextServices(
        this IServiceCollection services, IConfiguration _configuration)
    {
        services.AddDbContext<StoreContext>(x =>
            x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<AppIdentityDbContext>(x =>
            x.UseSqlite(_configuration.GetConnectionString("IdentityConnection")));

        return services;
    }
}
