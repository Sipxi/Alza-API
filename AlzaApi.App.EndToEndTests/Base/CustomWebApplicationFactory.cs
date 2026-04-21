using AlzaApi.DAL;
using AlzaApi.DAL.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlzaApi.App.EndToEndTests.Base;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connString;

    public CustomWebApplicationFactory()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        _connString = config.GetConnectionString("TestConnection")
                      ?? throw new InvalidOperationException("TestConnection not found.");
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_connString));
        });

        builder.UseEnvironment("Testing");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.Migrate();

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product
                {
                    Name = "Test Product 1", Description = "Desc 1", Price = 10.0m,
                    ImgUri = "https://example.com/product1.jpg"
                },
                new Product
                {
                    Name = "Test Product 2", Description = "Desc 2", Price = 20.0m,
                    ImgUri = "https://example.com/product2.jpg"
                }
            );
            db.SaveChanges();
        }

        return host;
    }
}
