using AlzaApi.DAL.Entities;
using AlzaApi.DAL.IntegrationTests.Fixtures;
using AlzaApi.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AlzaApi.DAL.IntegrationTests;

public class ProductRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly AppDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repository = new ProductRepository(_context);

        // Clear db before test
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        await _context.Products.AddRangeAsync(
            new Product { Id = Guid.NewGuid(), Name = "Item 1", Price = 10, ImgUri = "url" },
            new Product { Id = Guid.NewGuid(), Name = "Item 2", Price = 20, ImgUri = "url" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenItExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        await _context.Products.AddAsync(new Product
            { Id = id, Name = "Target Item", Price = 100, ImgUri = "url" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Target Item", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPaginatedAsync_ShouldReturnCorrectPage()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "A", Price = 10, ImgUri = "url" },
            new() { Id = Guid.NewGuid(), Name = "B", Price = 20, ImgUri = "url" },
            new() { Id = Guid.NewGuid(), Name = "C", Price = 30, ImgUri = "url" }
        };
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllPaginatedAsync(pageNumber: 1, pageSize: 2);

        // Assert
        Assert.Equal(3, result.TotalItems);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public async Task UpdateProductDescriptionAsync_ShouldChangeOnlyDescription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var product = new Product
            { Id = id, Name = "Phone", Price = 500, ImgUri = "url", Description = "Old Desc" };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        _context.Entry(product).State = EntityState.Detached;

        // Act
        await _repository.UpdateProductDescriptionAsync(id, "New Awesome Description");

        // Assert
        var dbItem = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        Assert.NotNull(dbItem);
        Assert.Equal("Phone", dbItem.Name);
        Assert.Equal(500, dbItem.Price);
        Assert.Equal("New Awesome Description", dbItem.Description);
    }
}
