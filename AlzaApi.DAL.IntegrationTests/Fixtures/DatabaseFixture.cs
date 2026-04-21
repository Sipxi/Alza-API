using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AlzaApi.DAL.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext Context { get; }

    public DatabaseFixture()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        string connString = config.GetConnectionString("TestConnection")
                            ?? throw new InvalidOperationException("TestConnection not found.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connString)
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
