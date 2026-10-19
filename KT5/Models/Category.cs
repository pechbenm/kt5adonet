using KT5.Models;

namespace KT5.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<Product> Products { get; set; } = new();
}