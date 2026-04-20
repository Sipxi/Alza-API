// AlzaApi.BL.UnitTests/ProductServiceTests.cs

using AlzaApi.BL.Services;
using AlzaApi.Common.DTOs.Product;
using AlzaApi.Common.Models;
using AlzaApi.DAL.Entities;
using AlzaApi.DAL.Interfaces;

using Moq;

namespace AlzaApi.BL.UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();

        _productService = new ProductService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productEntity = new Product
        {
            Id = productId,
            Name = "Test PC",
            Price = 1000,
            Description = "A great PC"
        };

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(productEntity);

        // Act
        var result = await _productService.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Test PC", result.Name);
        Assert.Equal("A great PC", result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?) null);

        // Act
        var result = await _productService.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenProductNotFound()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product?) null);

        string description = "New Desc";
        // Act
        var result = await _productService.UpdateDescriptionAsync(Guid.NewGuid(), description);

        // Assert
        Assert.False(result);

        _mockRepository.Verify(
            repo => repo.UpdateProductDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenProductUpdatedSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productEntity = new Product { Id = productId, Name = "Test" };

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(productEntity);

        string description = "New Desc";

        // Act
        var result = await _productService.UpdateDescriptionAsync(productId, description);

        // Assert
        Assert.True(result);

        _mockRepository.Verify(repo => repo.UpdateProductDescriptionAsync(productId, "New Desc"),
            Times.Once);
    }

    [Fact]
    public async Task GetAllPaginatedAsync_ShouldMapEntitiesToDtosCorrectly()
    {
        // Arrange
        var entities = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Item 1", Price = 10 },
            new() { Id = Guid.NewGuid(), Name = "Item 2", Price = 20 }
        };
        var paginatedEntities = new PaginatedResult<Product>(entities, 2, 1, 10);

        _mockRepository
            .Setup(repo => repo.GetAllPaginatedAsync(1, 10))
            .ReturnsAsync(paginatedEntities);

        // Act
        var result = await _productService.GetAllPaginatedAsync(1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.Items.Count());
        Assert.IsType<ProductListDto>(result.Items.First());
        Assert.Equal("Item 1", result.Items.First().Name);
    }
}
