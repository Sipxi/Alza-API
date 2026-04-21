using AlzaApi.DAL.Entities;
using AlzaApi.DAL.Interfaces;

namespace AlzaApi.DAL.Repositories;

public class ProductRepository(AppDbContext dbContext) : BaseRepository<Product>(dbContext),
    IProductRepository
{
    public async Task<bool> UpdateProductDescriptionAsync(Guid productId, string description)
    {
        var product = await _dbSet.FindAsync(productId);
        if (product is null) return false;
        
        product.Description = description;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
