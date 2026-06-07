using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess.Repositories.Implementations;

public class ReaderRepository : IReaderRepository
{
    private readonly LibraryContext _context;

    public ReaderRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        return await _context.Readers.ToListAsync();
    }

    public async Task<Reader?> GetByIdAsync(int id)
    {
        return await _context.Readers.FindAsync(id);
    }

    public async Task<Reader> AddAsync(Reader entity)
    {
        await _context.Readers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Reader entity)
    {
        _context.Readers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reader entity)
    {
        _context.Readers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Readers.AnyAsync(x => x.ReaderId == id);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeReaderId = null)
    {
        // TODO: Use this from the service to return Conflict on duplicates.
        return await _context.Readers.AnyAsync(x =>
            x.Email == email && (!excludeReaderId.HasValue || x.ReaderId != excludeReaderId.Value));
    }
}
