using FluentValidation;
using LibraryManager.DTOs.Readers;

namespace LibraryManager.Validators.Readers;

public class ReaderUpdateValidator : AbstractValidator<ReaderUpdateDto>
{
    public ReaderUpdateValidator()
    {
        RuleFor(x => x.ReaderId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(30);
    }
}
