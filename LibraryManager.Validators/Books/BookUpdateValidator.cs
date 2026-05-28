using FluentValidation;
using LibraryManager.DTOs.Books;

namespace LibraryManager.Validators.Books;

public class BookUpdateValidator : AbstractValidator<BookUpdateDto>
{
    public BookUpdateValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MaximumLength(30);
        RuleFor(x => x.PublicationYear).GreaterThan(0);
        RuleFor(x => x.AvailableCopies).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AuthorId).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}
