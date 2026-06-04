using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Readers;
using LibraryManager.Models.Entities;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class ReaderService : IReaderService
{
    private readonly IReaderRepository _repository;

    public ReaderService(IReaderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReaderDto>> GetAllAsync()
    {
        var readers = await _repository.GetAllAsync();
        return readers.Select(r => new ReaderDto
        {
            ReaderId = r.ReaderId,
            Name = r.Name,
            Email = r.Email,
            Phone = r.Phone
        });
    }

    public async Task<ReaderDto?> GetByIdAsync(int id)
    {
        var reader = await _repository.GetByIdAsync(id);

        if (reader == null)
        {
            return null;
        }

        return new ReaderDto
        {
            ReaderId = reader.ReaderId,
            Name = reader.Name,
            Email = reader.Email,
            Phone = reader.Phone
        };
    }

    public async Task<ReaderDto> CreateAsync(ReaderInsertDto dto)
    {  

        var emailExists = await _repository.EmailExistsAsync(dto.Email);

        if (emailExists)
        {
            throw new InvalidOperationException("A reader with the same email already exists.");
        }

        var reader = new Reader
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone
        };

        reader = await _repository.AddAsync(reader);

        return new ReaderDto
        {
            ReaderId = reader.ReaderId,
            Name = reader.Name,
            Email = reader.Email.Trim().ToLower(),
            Phone = reader.Phone
        };
    }

    public async Task<bool> UpdateAsync(int id, ReaderUpdateDto dto)
    {
        if (id != dto.ReaderId)
        {
            throw new ArgumentException("ID in URL does not match ID in body.");
        }

        var reader = await _repository.GetByIdAsync(id);
        if (reader == null)
        {
            return false;
        }

        var emailExists = await _repository.EmailExistsAsync(dto.Email, id);

        if (emailExists)
        {
            throw new InvalidOperationException("A reader with the same email already exists.");
        }

        reader.Name = dto.Name;
        reader.Email = dto.Email.Trim().ToLower();
        reader.Phone = dto.Phone;

        await _repository.UpdateAsync(reader);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // TODO: Check active loans before deleting.
        var reader = await _repository.GetByIdAsync(id);
        if (reader == null) 
        {
            return false;
        }
        await _repository.DeleteAsync(reader);
        return true;


    }
}
