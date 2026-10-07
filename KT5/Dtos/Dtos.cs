using System.ComponentModel.DataAnnotations;

namespace KT5.DTOs;

public record CategoryDto(int Id, string Name, string? Description);

public record ProductDto(int Id, string Name, string? Description,
                         decimal Price, int Stock, int CategoryId, string? CategoryName);

public class CategoryRequest
{
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class ProductRequest
{
    [Required, StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}