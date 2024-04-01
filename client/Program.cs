using Microsoft.OpenApi.Models;
using client.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using client.Data.Mappings.Interfaces;
using client.Data.Mappings;
using client.Data.Repositories.Interfaces;
using client.Data.Repositories;
using client.Services.Interfaces;
using client.Services;
using System.Collections.Generic;

namespace client
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
                    new ClientMap()
                };

                var connection = new DataContext(connectionString, entityMaps);
                connection.OnModelCreating();

                return connection;
            });
            
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<IClientService, ClientService>();
        
            builder.Services.AddEndpointsApiExplorer();
        
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "client", Version = "v1" });
            });
            
            builder.Services.AddControllers();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "client");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });

        var port = "8080";
        app.Run($"http://0.0.0.0:{port}");
        }
    }
}
