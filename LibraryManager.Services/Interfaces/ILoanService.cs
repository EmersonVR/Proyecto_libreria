using LibraryManager.DTOs.Loans;

namespace LibraryManager.Services.Interfaces;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllAsync();
    Task<LoanDto?> GetByIdAsync(int id);
    Task<LoanDto> CreateAsync(LoanInsertDto dto);
    Task<bool> UpdateAsync(int id, LoanInsertDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<LoanDto>> GetActiveAsync();
    Task<IEnumerable<LoanDto>> GetByReaderAsync(int readerId);
    Task<bool> ReturnAsync(int id);
}
