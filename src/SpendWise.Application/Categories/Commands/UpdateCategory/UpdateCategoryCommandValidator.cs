using FluentValidation;

namespace SpendWise.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(40).WithMessage("Category name must not exceed 40 characters.");

        RuleFor(x => x.Icon)
            .MaximumLength(20).WithMessage("Icon must not exceed 20 characters.");
    }
}
