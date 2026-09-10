using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class BaseRepository<T>(OrderContext context):IAsyncRepository<T> where T :BaseEntity
{
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
    {
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
     await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}