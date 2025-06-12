using E_Commerce.Web.Extensions;
using Persistence;
using Service;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            #region Add services to the container
		
            builder.Services.AddControllers();
            builder.Services.AddCors(options => 
            {
                options.AddPolicy("AllowAll" , builder => 
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            await app.SeedDataSeedAsync();

            #region Configure the HTTP request pipeline

            app.UseCustomExceptionMiddleWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); 

            #endregion

            app.Run();


        }
    }
}
