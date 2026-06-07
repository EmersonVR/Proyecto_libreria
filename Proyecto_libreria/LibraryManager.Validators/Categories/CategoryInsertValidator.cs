using FluentValidation;
using LibraryManager.DTOs.Categories;

namespace LibraryManager.Validators.Categories;

public class CategoryInsertValidator : AbstractValidator<CategoryInsertDto>
{
    public CategoryInsertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(300);
        // TODO: Check duplicate category name in service layer.
    }
}
