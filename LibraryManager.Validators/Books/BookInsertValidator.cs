using FluentValidation;
using LibraryManager.DTOs.Books;

namespace LibraryManager.Validators.Books;

public class BookInsertValidator : AbstractValidator<BookInsertDto>
{
    public BookInsertValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MaximumLength(30);
        RuleFor(x => x.PublicationYear).GreaterThan(0);
        RuleFor(x => x.AvailableCopies).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AuthorId).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        // TODO: Check ISBN uniqueness, AuthorId and CategoryId existence in service layer.
    }
}
