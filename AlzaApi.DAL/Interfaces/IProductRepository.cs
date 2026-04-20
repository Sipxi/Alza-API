using AlzaApi.DAL.Entities;

namespace AlzaApi.DAL.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task UpdateProductDescriptionAsync(Guid productId, string description);
}
