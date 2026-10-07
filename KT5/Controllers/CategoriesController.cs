using KT5.DTOs;
using KT5.Models;
using KT5.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KT5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public CategoriesController(IUnitOfWork uow) => _uow = uow;

    private static CategoryDto ToDto(Category c) => new(c.Id, c.Name, c.Description);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var items = await _uow.Categories.GetAllAsync();
        return Ok(items.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _uow.Categories.GetByIdAsync(id);
        if (category is null) return NotFound();
        return ToDto(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CategoryRequest request)
    {
        if (await _uow.Categories.AnyAsync(c => c.Name == request.Name))
            return Conflict($"категория '{request.Name}' уже существует.");

        var entity = new Category { Name = request.Name, Description = request.Description };
        await _uow.Categories.AddAsync(entity);
        await _uow.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequest request)
    {
        var entity = await _uow.Categories.GetByIdAsync(id);
        if (entity is null) return NotFound();

        if (await _uow.Categories.AnyAsync(c => c.Name == request.Name && c.Id != id))
            return Conflict($"категория '{request.Name}' уже существует.");

        entity.Name = request.Name;
        entity.Description = request.Description;
        _uow.Categories.Update(entity);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _uow.Categories.GetByIdAsync(id);
        if (entity is null) return NotFound();

        if (await _uow.Products.AnyAsync(p => p.CategoryId == id))
            return Conflict("нельзя удалить категорию, в которой есть товары.");

        _uow.Categories.Remove(entity);
        await _uow.SaveChangesAsync();

        return NoContent();
    }
}