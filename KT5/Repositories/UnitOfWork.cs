using KT5.Data;
using KT5.Models;
using KT5.Repositories;


namespace KT5.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Products = new Repository<Product>(context);
        Categories = new Repository<Category>(context);
    }

    public IRepository<Product> Products { get; }
    public IRepository<Category> Categories { get; }

    // один DbContext на запрос: все репозитории сохраняют изменения одной транзакцией.
    // жизненным циклом контекста управляет DI-контейнер, поэтому Dispose здесь не нужен.
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}