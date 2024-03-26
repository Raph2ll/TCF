using Microsoft.OpenApi.Models;
using api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using api.Data.Mappings.Interfaces;
using api.Data.Mappings;
using api.Data.Repositories.Interfaces;
using api.Data.Repositories;
using api.Services.Interfaces;
using api.Services;
using System.Collections.Generic;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddSingleton<DataContext>(_ =>
            {
                var entityMaps = new List<IEntityMap>
                {
                    new ClientMap()
                };

                var connection = new DataContext("Server=localhost;Port=3306;Uid=root;Pwd=user123", entityMaps);
                connection.OnModelCreating();

                return connection;
            });
            
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<IClientService, ClientService>();
        
            builder.Services.AddEndpointsApiExplorer();
        
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TCF", Version = "v1" });
            });
            
            builder.Services.AddControllers();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TCF");
                });
            }

            app.UseRouting(); // Adiciona o middleware de roteamento

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });

            app.Run();
        }
    }
}
