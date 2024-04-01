namespace product;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        var port = "7070";
        app.Run($"http://0.0.0.0:{port}");
    }
}