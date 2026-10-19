using KT5.DTOs;
using KT5.Models;
using KT5.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace ShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public ProductsController(IUnitOfWork uow) => _uow = uow;

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CategoryId, p.Category?.Name);

    // GET api/products  или  GET api/products?categoryId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] int? categoryId)
    {
        var items = categoryId is null
            ? await _uow.Products.GetAllAsync(p => p.Category)
            : await _uow.Products.FindAsync(p => p.CategoryId == categoryId, p => p.Category);

        return Ok(items.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _uow.Products.GetByIdAsync(id, p => p.Category);
        if (product is null) return NotFound();
        return ToDto(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductRequest request)
    {
        if (!await _uow.Categories.AnyAsync(c => c.Id == request.CategoryId))
            return BadRequest($"Категория с Id={request.CategoryId} не найдена.");

        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };

        await _uow.Products.AddAsync(entity);
        await _uow.SaveChangesAsync();

        var created = await _uow.Products.GetByIdAsync(entity.Id, p => p.Category);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(created!));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductRequest request)
    {
        var entity = await _uow.Products.GetByIdAsync(id);
        if (entity is null) return NotFound();

        if (!await _uow.Categories.AnyAsync(c => c.Id == request.CategoryId))
            return BadRequest($"Категория с Id={request.CategoryId} не найдена.");

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;
        entity.Stock = request.Stock;
        entity.CategoryId = request.CategoryId;

        _uow.Products.Update(entity);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _uow.Products.GetByIdAsync(id);
        if (entity is null) return NotFound();

        _uow.Products.Remove(entity);
        await _uow.SaveChangesAsync();

        return NoContent();
    }
}