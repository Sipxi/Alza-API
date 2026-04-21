using AlzaApi.Common.Models;
using AlzaApi.DAL.Entities;
using AlzaApi.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AlzaApi.DAL.Repositories;

public abstract class BaseRepository<TBaseEntity> : IBaseRepository<TBaseEntity>
    where TBaseEntity : BaseEntity
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TBaseEntity> _dbSet;

    protected BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TBaseEntity>();
    }

    public virtual async Task<IEnumerable<TBaseEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public Task<TBaseEntity?> GetByIdAsync(Guid id)
    {
        return _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<PaginatedResult<TBaseEntity>> GetAllPaginatedAsync(int pageNumber, int 
            pageSize)
    {
        var query = _dbSet.AsNoTracking();
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedResult<TBaseEntity>(items, totalCount, pageNumber, pageSize);
    }
    
}
