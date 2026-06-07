using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly LibraryContext _context;

    public CategoryRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> AddAsync(Category entity)
    {
        await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Category entity)
    {
        _context.Categories.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(x => x.CategoryId == id);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null)
    {
        // TODO: Use this from the service to return Conflict on duplicates.
        return await _context.Categories.AnyAsync(x =>
            x.Name == name && (!excludeCategoryId.HasValue || x.CategoryId != excludeCategoryId.Value));
    }
}
