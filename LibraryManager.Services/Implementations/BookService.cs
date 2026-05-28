using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Books;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BookDto>> GetAllAsync()
    {
        // TODO: Map Book entities manually to BookDto.
        throw new NotImplementedException("TODO: Implement Book GetAllAsync.");
    }

    public Task<BookDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("TODO: Implement Book GetByIdAsync.");
    }

    public Task<BookDto> CreateAsync(BookInsertDto dto)
    {
        // TODO: Validate ISBN uniqueness, author/category existence and map manually.
        throw new NotImplementedException("TODO: Implement Book CreateAsync.");
    }

    public Task<bool> UpdateAsync(int id, BookUpdateDto dto)
    {
        throw new NotImplementedException("TODO: Implement Book UpdateAsync.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        // TODO: Check active loans before deleting.
        throw new NotImplementedException("TODO: Implement Book DeleteAsync.");
    }

    public Task<IEnumerable<BookDto>> SearchByTitleAsync(string title)
    {
        // TODO: Use repository SearchByTitleAsync and map manually.
        throw new NotImplementedException("TODO: Implement book search service.");
    }

    public Task<IEnumerable<BookDto>> GetByAuthorAsync(int authorId)
    {
        throw new NotImplementedException("TODO: Implement books by author service.");
    }

    public Task<IEnumerable<BookDto>> GetByCategoryAsync(int categoryId)
    {
        throw new NotImplementedException("TODO: Implement books by category service.");
    }

    public Task<IEnumerable<BookDto>> GetAvailableAsync()
    {
        throw new NotImplementedException("TODO: Implement available books service.");
    }
}
