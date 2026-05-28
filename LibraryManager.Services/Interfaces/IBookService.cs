using LibraryManager.DTOs.Books;

namespace LibraryManager.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(BookInsertDto dto);
    Task<bool> UpdateAsync(int id, BookUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<BookDto>> SearchByTitleAsync(string title);
    Task<IEnumerable<BookDto>> GetByAuthorAsync(int authorId);
    Task<IEnumerable<BookDto>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<BookDto>> GetAvailableAsync();
}
