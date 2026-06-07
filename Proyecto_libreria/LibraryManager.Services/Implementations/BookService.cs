using LibraryManager.DataAccess.Repositories.Implementations;
using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Books;
using LibraryManager.Models.Entities;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILoanRepository _loanRepository;

    public BookService(IBookRepository repository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, ILoanRepository loanRepository)
    {
        _repository = repository;
        _authorRepository = authorRepository;
        _categoryRepository = categoryRepository;
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        // TODO: Map Book entities manually to BookDto.
        var books = await _repository.GetAllAsync();

        return books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Isbn = b.Isbn,
            PublicationYear = b.PublicationYear,
            AvailableCopies = b.AvailableCopies,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name, // Assuming Author navigation property exists
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name // Assuming Category navigation property exists
        });
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null) return null;

        return new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationYear,
            AvailableCopies = book.AvailableCopies,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.Name,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name
        };
    }

    public async Task<BookDto> CreateAsync(BookInsertDto dto)
    {
        // TODO: Validate ISBN uniqueness, author/category existence and map manually.       

        var existsIsbn = await _repository.IsbnExistsAsync(dto.Isbn);

        if (existsIsbn)
        {
            throw new InvalidOperationException("A book with the same ISBN already exists.");
        }

        var existingAuthor = await _authorRepository.GetByIdAsync(dto.AuthorId);

        if (existingAuthor == null)
        {
            throw new InvalidOperationException("The specified author does not exist.");
        }

        var existingCategory = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (existingCategory == null)
        {
            throw new InvalidOperationException("The specified category does not exist.");
        }

        var book = new Book
        {
            Title = dto.Title,
            Isbn = dto.Isbn,
            PublicationYear = dto.PublicationYear,
            AvailableCopies = dto.AvailableCopies,
            AuthorId = dto.AuthorId,
            CategoryId = dto.CategoryId
        };

        book = await _repository.AddAsync(book);

        return new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationYear,
            AvailableCopies = book.AvailableCopies,
            AuthorId = book.AuthorId,
            AuthorName = existingAuthor.Name,
            CategoryId = book.CategoryId,
            CategoryName = existingCategory.Name,
        };
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
    {
        if (id != dto.BookId)
        {
            throw new ArgumentException("ID in URL does not match ID in body.");
        }

        var book = await _repository.GetByIdAsync(id);

        if (book == null)
        {
            return false;
        }

        var existsIsbn = await _repository.IsbnExistsAsync(dto.Isbn, id);
        if (existsIsbn)
        {
            throw new InvalidOperationException("A book with the same ISBN already exists.");
        }

        var existingAuthor = await _authorRepository.GetByIdAsync(dto.AuthorId);
        if (existingAuthor == null)
        {
            throw new InvalidOperationException("The specified author does not exist.");
        }

        var existingCategory = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (existingCategory == null)
        {
            throw new InvalidOperationException("The specified category does not exist.");
        }

        // Update the book properties
        book.Title = dto.Title;
        book.Isbn = dto.Isbn;
        book.PublicationYear = dto.PublicationYear;
        book.AvailableCopies = dto.AvailableCopies;
        book.AuthorId = dto.AuthorId;
        book.CategoryId = dto.CategoryId;

        await _repository.UpdateAsync(book);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // TODO: Validate that the book has no active loans when Loans module is implemented.
        var book = await _repository.GetByIdAsync(id);
        if (book == null)
        {
            return false;
        }

        var activeLoans = await _loanRepository.GetActiveAsync();

        if (activeLoans.Any(l => l.BookId == id))
        {
            throw new InvalidOperationException("Cannot delete a book with active loans.");
        }

        await _repository.DeleteAsync(book);
        return true;
    }

    public async Task<IEnumerable<BookDto>> SearchByTitleAsync(string title)
    {
        var books = await _repository.SearchByTitleAsync(title);
        return books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Isbn = b.Isbn,
            PublicationYear = b.PublicationYear,
            AvailableCopies = b.AvailableCopies,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        });
    }

    public async Task<IEnumerable<BookDto>> GetByAuthorAsync(int authorId)
    {
        var books = await _repository.GetByAuthorAsync(authorId);
        return books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Isbn = b.Isbn,
            PublicationYear = b.PublicationYear,
            AvailableCopies = b.AvailableCopies,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        });
    }

    public async Task<IEnumerable<BookDto>> GetByCategoryAsync(int categoryId)
    {
        var books = await _repository.GetByCategoryAsync(categoryId);
        return books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Isbn = b.Isbn,
            PublicationYear = b.PublicationYear,
            AvailableCopies = b.AvailableCopies,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        });
    }

    public async Task<IEnumerable<BookDto>> GetAvailableAsync()
    {
        var books = await _repository.GetAvailableAsync();
        return books.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Isbn = b.Isbn,
            PublicationYear = b.PublicationYear,
            AvailableCopies = b.AvailableCopies,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        });
    }
}
