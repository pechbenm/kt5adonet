using KT5.Data;
using KT5.Models;
using KT5.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KT5.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _set;

    public Repository(AppDbContext context) => _set = context.Set<T>();

    private static IQueryable<T> ApplyIncludes(IQueryable<T> query,
                                               Expression<Func<T, object?>>[] includes)
    {
        foreach (var include in includes)
            query = query.Include(include);
        return query;
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes)
        => await ApplyIncludes(_set.AsNoTracking(), includes).ToListAsync();

    public Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes)
        => ApplyIncludes(_set, includes).FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                                params Expression<Func<T, object?>>[] includes)
        => await ApplyIncludes(_set.AsNoTracking(), includes).Where(predicate).ToListAsync();

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => _set.AnyAsync(predicate);

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
}