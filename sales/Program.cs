using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Serilog;
using Refit;
using MongoDB.Driver;
using sales.src.Repositories;
using sales.src.Repositories.Interfaces;
using sales.src.Services;
using sales.src.Services.Interfaces;
using sales.src.Services.Refit;

namespace sales
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

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase("sales_db");
            builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

            builder.Services.AddScoped<ISaleRepository, SaleRepository>();
            builder.Services.AddScoped<ISaleService, SaleService>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "product",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Raph2ll",
                        Email = "raph2ll@gmail.com",
                        Url = new Uri("https://github.com/Raph2ll")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddControllers();

            builder.Services.AddRefitClient<IClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://client:8080"));

            builder.Services.AddRefitClient<IProduct>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://product:7070"));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate:
                "{Timestamp:yyyy-MM-ddTHH:mm:ssZ} {Level:u}\t{Message:lj} {NewLine}{Exception}")
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
            });

            builder.Host.UseSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "sales");
                });
            }

            // app.UseSerilogRequestLogging();
            app.UseRouting();

            app.MapControllers();
            app.Run($"http://0.0.0.0:8080");
        }
    }
}