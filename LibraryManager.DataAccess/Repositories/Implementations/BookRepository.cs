using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.BookId == id);
    }

    public async Task<Book> AddAsync(Book entity)
    {
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book entity)
    {
        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Books.AnyAsync(x => x.BookId == id);
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
    {
        // TODO: Practice Where, Select, OrderBy, Any, FirstOrDefaultAsync and ToListAsync here.
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Where(x => x.Title.Contains(title))
            .OrderBy(x => x.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
    {
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Where(x => x.AuthorId == authorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAvailableAsync()
    {
        return await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Where(x => x.AvailableCopies > 0)
            .ToListAsync();
    }

    public async Task<bool> IsbnExistsAsync(string isbn, int? excludeBookId = null)
    {
        return await _context.Books.AnyAsync(x =>
            x.Isbn == isbn && (!excludeBookId.HasValue || x.BookId != excludeBookId.Value));
    }
}
