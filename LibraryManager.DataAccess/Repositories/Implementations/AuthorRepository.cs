using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess.Repositories.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryContext _context;

    public AuthorRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        // TODO: Add ordering, filters or includes when the feature needs them.
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> AddAsync(Author entity)
    {
        await _context.Authors.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Author entity)
    {
        // TODO: Decide which fields can be updated from the service layer.
        _context.Authors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Author entity)
    {
        // TODO: Validate business rules in the service before deleting.
        _context.Authors.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Authors.AnyAsync(x => x.AuthorId == id);
    }
}
