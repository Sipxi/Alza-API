using Microsoft.EntityFrameworkCore;

namespace AlzaApi.DAL.Seeds;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();
        await SeedProductsAsync(context);
    }

    private static async Task SeedProductsAsync(AppDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(ProductSeeder.GetSeedData());
            await context.SaveChangesAsync();
        }
    }
}
