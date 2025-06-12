using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Persistence
{
    static public class InfrastructureServicesRegistration
    {

        static public IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("IdentityConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepositories, BasketRepositories>();
            services.AddScoped<ICashRepository, CashRepository>();

            services.AddSingleton<IConnectionMultiplexer>((_) => 
            {
              return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnectionString"));
            });

            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }

    }
}
