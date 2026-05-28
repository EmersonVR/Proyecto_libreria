using LibraryManager.DTOs.Authors;

namespace LibraryManager.Services.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(AuthorInsertDto dto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
