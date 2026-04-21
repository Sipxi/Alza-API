using AlzaApi.BL.Interfaces;
using AlzaApi.Common.DTOs.Product;
using AlzaApi.Common.Models;
using AlzaApi.DAL.Interfaces;

using Mapster;

namespace AlzaApi.BL.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IEnumerable<ProductListDto>> GetAllAsync()
    {
        var products = await productRepository.GetAllAsync();
        return products.Adapt<IEnumerable<ProductListDto>>();
    }

    public async Task<ProductDetailDto?> GetByIdAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id);
        return product?.Adapt<ProductDetailDto>();
    }

    public async Task<bool> UpdateDescriptionAsync(Guid id, string description)
    {
        return await productRepository.UpdateProductDescriptionAsync(id, description);
    }

    public async Task<PaginatedResult<ProductListDto>> GetAllPaginatedAsync(int pageNumber, int
        pageSize)
    {
        var pagedEntities = await productRepository.GetAllPaginatedAsync(pageNumber, pageSize);
        var pagedDtos = pagedEntities.Items.Adapt<IEnumerable<ProductListDto>>();
        return new PaginatedResult<ProductListDto>(pagedDtos, pagedEntities.TotalItems,
            pagedEntities.PageNumber, pagedEntities.PageSize);
    }
}
