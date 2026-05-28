using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Authors;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;

    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        // TODO: Read entities from repository and map manually to DTOs.
        throw new NotImplementedException("TODO: Implement Author GetAllAsync.");
    }

    public Task<AuthorDto?> GetByIdAsync(int id)
    {
        // TODO: Return null when repository does not find the entity.
        throw new NotImplementedException("TODO: Implement Author GetByIdAsync.");
    }

    public Task<AuthorDto> CreateAsync(AuthorInsertDto dto)
    {
        // TODO: Create Author entity, save it through repository and map manually to AuthorDto.
        throw new NotImplementedException("TODO: Implement Author CreateAsync.");
    }

    public Task<bool> UpdateAsync(int id, AuthorUpdateDto dto)
    {
        // TODO: Check route id, find entity, update allowed fields and save.
        throw new NotImplementedException("TODO: Implement Author UpdateAsync.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        // TODO: Check that the author has no books before deleting.
        throw new NotImplementedException("TODO: Implement Author DeleteAsync.");
    }
}
