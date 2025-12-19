using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(
    Guid Id,
    Guid UserId) : ICommand<Guid>;
