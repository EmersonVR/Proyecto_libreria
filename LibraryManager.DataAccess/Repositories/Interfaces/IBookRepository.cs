using LibraryManager.Models.Entities;

namespace LibraryManager.DataAccess.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> AddAsync(Book entity);
    Task UpdateAsync(Book entity);
    Task DeleteAsync(Book entity);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Book>> SearchByTitleAsync(string title);
    Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
    Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Book>> GetAvailableAsync();
    Task<bool> IsbnExistsAsync(string isbn, int? excludeBookId = null);
}
