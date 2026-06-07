using FluentValidation;
using LibraryManager.DTOs.Authors;

namespace LibraryManager.Validators.Authors;

public class AuthorUpdateValidator : AbstractValidator<AuthorUpdateDto>
{
    public AuthorUpdateValidator()
    {
        RuleFor(x => x.AuthorId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(150);
    }
}
