using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.Models.Entities;
using LibraryManager.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess.Repositories.Implementations;

public class LoanRepository : ILoanRepository
{
    private readonly LibraryContext _context;

    public LoanRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _context.Loans.Include(x => x.Book).Include(x => x.Reader).ToListAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _context.Loans
            .Include(x => x.Book)
            .Include(x => x.Reader)
            .FirstOrDefaultAsync(x => x.LoanId == id);
    }

    public async Task<Loan> AddAsync(Loan entity)
    {
        await _context.Loans.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Loan entity)
    {
        _context.Loans.Update(entity);
        await _context.SaveChangesAsync();
    } 


    public async Task<IEnumerable<Loan>> GetActiveAsync()
    {
        return await _context.Loans.Include(x => x.Book).Include(x => x.Reader).Where(x => x.Status == LoanStatus.Active).ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByReaderAsync(int readerId)
    {
        return await _context.Loans.Include(x => x.Book).Include(x => x.Reader).Where(x => x.ReaderId == readerId).ToListAsync();
    }
}
