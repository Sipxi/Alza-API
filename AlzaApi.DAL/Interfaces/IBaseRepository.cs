using AlzaApi.Common.Models;
using AlzaApi.DAL.Entities;

namespace AlzaApi.DAL.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<PaginatedResult<TEntity>> GetAllPaginatedAsync(int pageNumber, int pageSize);
}
