using System.Text.Json;
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWare;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace E_Commerce.Web.Extensions
{
    static public class WebApplicationRegistration
    {
        static public async Task SeedDataSeedAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var ObjectDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectDataSeeding.DataSeedAsync();       
            await ObjectDataSeeding.IdentityDataSeedAsync();
        }

        static public IApplicationBuilder UseCustomExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
            return app;
        }

        static public IApplicationBuilder UseSwaggerMiddleWare(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };

                options.DocumentTitle = "My E-Commerce Api";

                options.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                options.DocExpansion(DocExpansion.None);
                options.EnableFilter();
                options.EnablePersistAuthorization();
            });

            return app;
        }


    }
}
