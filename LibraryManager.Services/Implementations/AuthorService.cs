using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Authors;
using LibraryManager.Models.Entities;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;

    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        // TODO: Read entities from repository and map manually to DTOs.
        var authors = await _repository.GetAllAsync();
        return authors.Select(a => new AuthorDto
        {
            AuthorId = a.AuthorId,
            Name = a.Name,
            BirthDate = a.BirthDate
        });

    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        // TODO: Return null when repository does not find the entity.
        var author = await _repository.GetByIdAsync(id);

        if (author is null)
        {
            return null;
        }

        return new AuthorDto
        {
            AuthorId = author.AuthorId,
            Name = author.Name,
            BirthDate = author.BirthDate
        };
    }

    public async Task<AuthorDto> CreateAsync(AuthorInsertDto dto)
    {
        // TODO: Create Author entity, save it through repository and map manually to AuthorDto

        var author = new Author
        {
            Name = dto.Name,
            BirthDate = dto.BirthDate
        };

        await _repository.AddAsync(author);

        return new AuthorDto
        {
            AuthorId = author.AuthorId,
            Name = author.Name,
            BirthDate = author.BirthDate
        };
    }

    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto dto)
    {
        // TODO: Check route id, find entity, update allowed fields and save.
        var author =  await _repository.GetByIdAsync(id);

        if (author is null)
        {
            return false;
        }

        author.Name = dto.Name;
        author.BirthDate = dto.BirthDate;
        await _repository.UpdateAsync(author);

        return true;

    }

    public async Task<bool> DeleteAsync(int id)
    {
        // TODO: Check that the author has no books before deleting.

        var author = await _repository.GetByIdAsync(id);

        if (author is null)
        {
            return false;
        }


        await _repository.DeleteAsync(author);
        return true;
    }
}
