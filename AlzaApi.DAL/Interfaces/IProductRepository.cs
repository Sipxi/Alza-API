using AlzaApi.DAL.Entities;

namespace AlzaApi.DAL.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> UpdateProductDescriptionAsync(Guid productId, string description);
}
