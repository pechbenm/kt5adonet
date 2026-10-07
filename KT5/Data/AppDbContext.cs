using KT5.Models;
using Microsoft.EntityFrameworkCore;

namespace KT5.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Description).HasMaxLength(500);
            e.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Description).HasMaxLength(1000);
            e.Property(p => p.Price).HasPrecision(18, 2);
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // начальные данные (Id задаются явно — требование hasdata)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Электроника", Description = "Гаджеты и техника" },
            new Category { Id = 2, Name = "Книги", Description = "Художественная и техническая литература" },
            new Category { Id = 3, Name = "Одежда" });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Смартфон", Price = 599.99m, Stock = 25, CategoryId = 1 },
            new Product { Id = 2, Name = "Ноутбук", Price = 1199.00m, Stock = 10, CategoryId = 1 },
            new Product { Id = 3, Name = "Книга по C#", Price = 34.50m, Stock = 100, CategoryId = 2 },
            new Product { Id = 4, Name = "Футболка", Price = 19.90m, Stock = 50, CategoryId = 3 });
    }
}