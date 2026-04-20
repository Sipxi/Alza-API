using AlzaApi.Common.DTOs.Product;
using AlzaApi.Common.Models;

namespace AlzaApi.BL.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductListDto>> GetAllAsync();   
    Task<ProductDetailDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, ProductUpdateDto updateDto);
    Task<PaginatedResult<ProductListDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
}
