using SpendWise.Application.Categories.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string CategoryName,
    string? Icon,
    Guid UserId) : ICommand<CategoryResponse>;