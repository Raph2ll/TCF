using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;
using quotation.src.Services.Refit;
using Refit;
using quotation.src.Services.Interfaces;
using quotation.src.Services;
using StackExchange.Redis;

namespace quotation
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
            string awesomeApiUrl = configuration["External:AwesomeApi"];

            var redis = ConnectionMultiplexer.Connect(connectionString);

            builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

            builder.Services.AddSingleton<IQuotationService, QuotationService>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "client",
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
            builder.Services.AddRefitClient<IEconomy>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(awesomeApiUrl));


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
                    c.SwaggerEndpoint("v1/swagger.json", "quotation");
                });
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.MapControllers();

            app.Run($"http://0.0.0.0:8080");
        }
    }
}
