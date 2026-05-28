using FluentValidation;
using LibraryManager.DTOs.Readers;

namespace LibraryManager.Validators.Readers;

public class ReaderInsertValidator : AbstractValidator<ReaderInsertDto>
{
    public ReaderInsertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(30);
    }
}
