using FluentValidation;
using LibraryManager.DTOs.Loans;

namespace LibraryManager.Validators.Loans;

public class LoanInsertValidator : AbstractValidator<LoanInsertDto>
{
    public LoanInsertValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.ReaderId).GreaterThan(0);
        // TODO: Check book availability and reader existence in service layer.
    }
}
