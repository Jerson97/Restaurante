using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Application.Interfaces.Token;
using Restaurant.Persistence.Context;
using Restaurant.Persistence.UOW;

namespace Restaurant.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresSQLConnection");

            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
