using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Loans;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;

    public LoanService(ILoanRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<LoanDto>> GetAllAsync()
    {
        throw new NotImplementedException("TODO: Implement Loan GetAllAsync.");
    }

    public Task<LoanDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("TODO: Implement Loan GetByIdAsync.");
    }

    public Task<LoanDto> CreateAsync(LoanInsertDto dto)
    {
        // TODO: Validate book, reader, available copies and reduce AvailableCopies.
        throw new NotImplementedException("TODO: Implement Loan CreateAsync.");
    }

    public Task<bool> UpdateAsync(int id, LoanInsertDto dto)
    {
        // TODO: Loans are usually not edited; decide if this remains necessary.
        throw new NotImplementedException("TODO: Implement Loan UpdateAsync.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException("TODO: Implement Loan DeleteAsync.");
    }

    public Task<IEnumerable<LoanDto>> GetActiveAsync()
    {
        throw new NotImplementedException("TODO: Implement active loans service.");
    }

    public Task<IEnumerable<LoanDto>> GetByReaderAsync(int readerId)
    {
        throw new NotImplementedException("TODO: Implement loans by reader service.");
    }

    public Task<bool> ReturnAsync(int id)
    {
        // TODO: Validate active loan, set ReturnDate, set Returned and increase AvailableCopies.
        throw new NotImplementedException("TODO: Implement return loan service.");
    }
}
