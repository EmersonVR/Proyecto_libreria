using LibraryManager.Models.Entities;

namespace LibraryManager.DataAccess.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<Author> AddAsync(Author entity);
    Task UpdateAsync(Author entity);
    Task DeleteAsync(Author entity);
    Task<bool> ExistsAsync(int id);
}
