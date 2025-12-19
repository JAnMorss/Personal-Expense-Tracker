using SpendWise.Application.Categories.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid Id,
    Guid UserId,
    string CategoryName,
    string? Icon) : ICommand<CategoryResponse>;
