using AlzaApi.DAL.Entities;

namespace AlzaApi.DAL.Seeds;

public static class ProductSeeder
{
    public static IEnumerable<Product> GetSeedData()
    {
        return new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Name = "MacBook Air M2",
                ImgUri = "https://alza.cz/foto/macbook",
                Price = 29990.00m,
                Description = "Macbook, black, new generation, with M2 chip.",
                CreatedAt = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc) 
            },
            new Product
            {
                Id = Guid.Parse("f9168c5e-ceb2-4faa-b6bf-329bf39fa1e4"),
                Name = "Gaming PC 2025",
                ImgUri = "https://alza.cz/foto/gaming-pc",
                Price = 35490.00m,
                Description = "Gaming PC RTX 2080",
                CreatedAt = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("114b3f8d-db81-4b1f-a5b6-76a0d24cb3d1"),
                Name = "Lenovo IdeaPad 3",
                ImgUri = "https://alza.cz/foto/lenovo-ideapad",
                Price = 27990.00m,
                Description = "Lenovo IdeaPad 3, 15.6\", Intel Core i5, 8GB RAM, 512GB SSD",
                CreatedAt = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}
