using System.Text.Json;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext , 
        UserManager<ApplicationUser> _userManager ,RoleManager<IdentityRole> _roleManager ,
        StoreIdentityDbContext _identityDbContext , ILogger<DataSeeding> _logger) 
        : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                   await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var productBrandData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var Brands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandData);
                    if (Brands is not null && Brands.Any())
                       await _dbContext.ProductBrands.AddRangeAsync(Brands);
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypeData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    var types = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypeData);
                    if (types is not null && types.Any())
                       await _dbContext.ProductTypes.AddRangeAsync(types);
                }

                if (!_dbContext.Products.Any())
                {
                    var productData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(productData);
                    if (products is not null && products.Any())
                       await _dbContext.Products.AddRangeAsync(products);
                }

                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliveryMethodStream = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodStream);
                
                    if(DeliveryMethods is not null && DeliveryMethods.Any())
                    {
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during data seeding.");
            }

        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Mohammed@gmail.com",
                        DisplayName = "Mohammed Adel",
                        PhoneNumber = "01018688784",
                        UserName = "MohammedAdel",
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Aliaa@gmail.com",
                        DisplayName = "Aliaa Tarek",
                        PhoneNumber = "01015588784",
                        UserName = "AliaaTarek",
                    };

                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }

            }
            catch (Exception e)
            {

            }

        }


    }
}
