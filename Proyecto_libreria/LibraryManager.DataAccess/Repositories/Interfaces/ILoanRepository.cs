using LibraryManager.Models.Entities;

namespace LibraryManager.DataAccess.Repositories.Interfaces;

public interface ILoanRepository
{
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan> AddAsync(Loan entity);
    Task UpdateAsync(Loan entity); 
    Task<IEnumerable<Loan>> GetActiveAsync();
    Task<IEnumerable<Loan>> GetByReaderAsync(int readerId);
}
