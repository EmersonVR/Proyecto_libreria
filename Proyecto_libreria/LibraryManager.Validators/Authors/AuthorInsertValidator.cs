using FluentValidation;
using LibraryManager.DTOs.Authors;

namespace LibraryManager.Validators.Authors;

public class AuthorInsertValidator : AbstractValidator<AuthorInsertDto>
{
    public AuthorInsertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(150);
        // TODO: Add date rules if needed.
    }
}
