using LibraryManager.DTOs.Readers;

namespace LibraryManager.Services.Interfaces;

public interface IReaderService
{
    Task<IEnumerable<ReaderDto>> GetAllAsync();
    Task<ReaderDto?> GetByIdAsync(int id);
    Task<ReaderDto> CreateAsync(ReaderInsertDto dto);
    Task<bool> UpdateAsync(int id, ReaderUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
