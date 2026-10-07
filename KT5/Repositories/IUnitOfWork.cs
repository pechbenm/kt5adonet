using KT5.Models;
using KT5.Repositories;

namespace KT5.Repositories;

public interface IUnitOfWork
{
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    Task<int> SaveChangesAsync();
}