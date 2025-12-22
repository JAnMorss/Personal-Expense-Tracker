using SpendWise.Application.Expenses.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.AddExpenseToCategory;

public sealed record AddExpenseToCategoryCommand(
    Guid CategoryId,
    Guid UserId,
    decimal Amount,
    DateTime Date,
    string? Description) : ICommand<ExpenseResponse>;
