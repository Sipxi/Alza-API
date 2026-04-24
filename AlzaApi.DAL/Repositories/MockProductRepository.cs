using AlzaApi.Common.Models;
using AlzaApi.DAL.Entities;
using AlzaApi.DAL.Interfaces;
using AlzaApi.DAL.Seeds;

namespace AlzaApi.DAL.Repositories;

public class MockProductRepository : IProductRepository
{
    private readonly List<Product> _products = ProductSeeder.GetSeedData().ToList();

    public Task<IEnumerable<Product>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Product>>(_products);

    public Task<Product?> GetByIdAsync(Guid id) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<PaginatedResult<Product>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        var items = _products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(new PaginatedResult<Product>(items, _products.Count, pageNumber, pageSize));
    }

    public Task<bool> UpdateProductDescriptionAsync(Guid id, string description)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null) product.Description = description;
        return Task.FromResult(product != null);
    }
    
}
