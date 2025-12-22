using SpendWise.Application.Expenses.Response;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdQuery(
    Guid Id,
    Guid UserId) : IQuery<ExpenseResponse>;
