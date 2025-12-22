using SpendWise.Application.Expenses.Response;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Application.Expenses.Queries.GetAllExpenses;

public sealed record GetAllExpensesQuery(
    QueryObject? Query,
    Guid UserId) : IQuery<PaginatedResult<ExpenseResponse>>;
