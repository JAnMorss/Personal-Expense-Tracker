using SpendWise.Application.Expenses.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid Id,
    Guid UserId,
    decimal Amount,
    DateTime Date,
    string? Description) : ICommand<ExpenseResponse>;

