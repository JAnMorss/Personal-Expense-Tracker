using FluentValidation;

namespace SpendWise.Application.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(40).WithMessage("Category name must not exceed 100 characters.");

        RuleFor(x => x.Icon)
            .MaximumLength(20).WithMessage("Icon must not exceed 200 characters.");
    }
}
