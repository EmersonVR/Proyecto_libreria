using LibraryManager.Models.Entities;

namespace LibraryManager.DataAccess.Repositories.Interfaces;

public interface IReaderRepository
{
    Task<IEnumerable<Reader>> GetAllAsync();
    Task<Reader?> GetByIdAsync(int id);
    Task<Reader> AddAsync(Reader entity);
    Task UpdateAsync(Reader entity);
    Task DeleteAsync(Reader entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeReaderId = null);
}
