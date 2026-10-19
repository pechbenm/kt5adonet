using KT5.Models;
using System.Linq.Expressions;

namespace KT5.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                   params Expression<Func<T, object?>>[] includes);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}