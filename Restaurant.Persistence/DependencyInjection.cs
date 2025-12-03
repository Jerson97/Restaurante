using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Persistence.Context;

namespace Restaurant.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresSQLConnection");

            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseNpgsql(connectionString));


            return services;
        }
    }
}
