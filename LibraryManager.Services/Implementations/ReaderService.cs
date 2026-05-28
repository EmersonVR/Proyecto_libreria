using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Readers;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class ReaderService : IReaderService
{
    private readonly IReaderRepository _repository;

    public ReaderService(IReaderRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ReaderDto>> GetAllAsync()
    {
        throw new NotImplementedException("TODO: Implement Reader GetAllAsync.");
    }

    public Task<ReaderDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("TODO: Implement Reader GetByIdAsync.");
    }

    public Task<ReaderDto> CreateAsync(ReaderInsertDto dto)
    {
        // TODO: Validate duplicate email and map manually.
        throw new NotImplementedException("TODO: Implement Reader CreateAsync.");
    }

    public Task<bool> UpdateAsync(int id, ReaderUpdateDto dto)
    {
        throw new NotImplementedException("TODO: Implement Reader UpdateAsync.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        // TODO: Check active loans before deleting.
        throw new NotImplementedException("TODO: Implement Reader DeleteAsync.");
    }
}
