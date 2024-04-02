using product.Data;
using product.Data.Mappings.Interfaces;
using product.Data.Mappings;
using product.Data.Repositories.Interfaces;
using product.Data.Repositories;
using product.Services.Interfaces;
using product.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddSingleton<DataContext>(_ =>
            {
                var entityMaps = new List<IEntityMap>
                {
                    new ProductMap()
                };

                var connection = new DataContext(connectionString, entityMaps);
                connection.OnModelCreating();

                return connection;
            });

            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IProductService, ProductService>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "product", Version = "v1" });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "product");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var port = "7070";
            app.Run($"http://0.0.0.0:{port}");
        }
    }
}