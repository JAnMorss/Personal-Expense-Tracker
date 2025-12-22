using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.DeleteExpense;

public sealed record DeleteExpenseCommand(
    Guid Id, 
    Guid UserId) : ICommand<Guid>;