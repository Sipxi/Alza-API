using AlzaApi.DAL;

using Microsoft.EntityFrameworkCore;

namespace AlzaApi.DAL.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext Context { get; }

    // Используем твою третью базу для тестов
    private const string _connString =
        "Server=SIPXI-LAPTOP;Database=AlzaDb_Test;Trusted_Connection=True;TrustServerCertificate=True;";

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connString)
            .Options;

        Context = new AppDbContext(options);

        // After the test, delete and recreate
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
