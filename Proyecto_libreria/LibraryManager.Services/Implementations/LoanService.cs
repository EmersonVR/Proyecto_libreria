using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Loans;
using LibraryManager.Models.Entities;
using LibraryManager.Models.Enums;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;
    private readonly IBookRepository _bookRepository;
    private readonly IReaderRepository _readerRepository;

    public LoanService(ILoanRepository repository, IBookRepository bookRepository, IReaderRepository readerRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;       

    }

    public async Task<IEnumerable<LoanDto>> GetAllAsync()
    {
        var loans = await _repository.GetAllAsync();

        return loans.Select(loan => new LoanDto
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title,
            ReaderId = loan.ReaderId,
            ReaderName = loan.Reader?.Name,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
        });
    }

    public async Task<LoanDto?> GetByIdAsync(int id)
    {
       var loan = await _repository.GetByIdAsync(id);

        if (loan == null)
        {
            return null;
        }

        return new LoanDto
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title,
            ReaderId = loan.ReaderId,
            ReaderName = loan.Reader?.Name,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
        };
    }

    public async Task<LoanDto> CreateAsync(LoanInsertDto dto)
    {
        var book = await _bookRepository.GetByIdAsync(dto.BookId);

        if (book == null)
        {
            throw new ArgumentException($"Book with ID {dto.BookId} does not exist.");
        }

        var reader = await _readerRepository.GetByIdAsync(dto.ReaderId);

        if (reader == null)
        {
            throw new ArgumentException($"Reader with ID {dto.ReaderId} does not exist.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException($"No available copies for book ID {dto.BookId}.");
        }

        var loan = new Loan
        {
            BookId = dto.BookId,
            ReaderId = dto.ReaderId,
            LoanDate = DateTime.UtcNow,
            Status = LoanStatus.Active
        };

        var createdLoan = await _repository.AddAsync(loan);

        book.AvailableCopies -= 1;
        await _bookRepository.UpdateAsync(book);

        return new LoanDto
        {
            LoanId = createdLoan.LoanId,
            BookId = createdLoan.BookId,
            BookTitle = book.Title,
            ReaderId = createdLoan.ReaderId,
            ReaderName = reader.Name,
            LoanDate = createdLoan.LoanDate,
            ReturnDate = createdLoan.ReturnDate,
            Status = createdLoan.Status.ToString()
        };
    }

    public async Task<IEnumerable<LoanDto>> GetActiveAsync()
    {
        var loans = await _repository.GetActiveAsync();

        return loans.Select(l => new LoanDto
        {
            LoanId = l.LoanId,
            BookId = l.BookId,
            BookTitle = l.Book?.Title,
            ReaderId = l.ReaderId,
            ReaderName = l.Reader?.Name,
            LoanDate = l.LoanDate,
            ReturnDate = l.ReturnDate,
            Status = l.Status.ToString(),
        });
    }

    public async Task<IEnumerable<LoanDto>> GetByReaderAsync(int readerId)
    {
        var reader = await _readerRepository.GetByIdAsync(readerId);

        if (reader == null)
        {
            throw new ArgumentException($"Reader with ID {readerId} does not exist.");
        }

        var loans = await _repository.GetByReaderAsync(readerId);

        return loans.Select(loan => new LoanDto
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title,
            ReaderId = loan.ReaderId,
            ReaderName = loan.Reader?.Name,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
        });
    }

    public async Task<bool> ReturnAsync(int id)
    {       
        var loan = await _repository.GetByIdAsync(id);

        if (loan == null)
        {
            return false;
        }

        if (loan.Status == LoanStatus.Returned)
        {
            throw new InvalidOperationException($"Loan with ID {id} is already returned.");
        }     
        
        if (loan.Book == null)
        {
            throw new InvalidOperationException($"Book for loan ID {id} is not loaded.");
        }

        loan.Book.AvailableCopies += 1;
        loan.Status = LoanStatus.Returned;
        loan.ReturnDate = DateTime.UtcNow;

        await _repository.UpdateAsync(loan);
        return true;
    }
}
