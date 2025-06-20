using ESIID42025.DTOs;
using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return null;
        return new CategoryDto
        {
            ID = c.ID,
            Name = c.Name,
            Description = c.Description
        };
    }

    public async Task AddCategoryAsync(CategoryDto category)
    {
        var entity = new Category
        {
            Name = category.Name,
            Description = category.Description,
            ID = category.ID
        };
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(CategoryDto category)
    {
        var entity = await _context.Categories.FindAsync(category.ID);
        if (entity != null)
        {
            entity.Name = category.Name;
            entity.Description = category.Description;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity != null)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}