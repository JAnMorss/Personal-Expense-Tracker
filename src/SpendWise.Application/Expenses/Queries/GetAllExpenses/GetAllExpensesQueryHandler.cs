using SpendWise.Application.Expenses.Response;
using SpendWise.Domain.Expenses.Errors;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Application.Expenses.Queries.GetAllExpenses;

public sealed class GetAllExpensesQueryHandler 
    : IQueryHandler<GetAllExpensesQuery, PaginatedResult<ExpenseResponse>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetAllExpensesQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<PaginatedResult<ExpenseResponse>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = request.Query ?? new QueryObject();

        var expenses = await _expenseRepository.GetAllAsync(
            query,
            request.UserId,
            cancellationToken);
        if (expenses is null || !expenses.Any())
            return Result.Failure<PaginatedResult<ExpenseResponse>>(ExpenseErrors.EmptyExpense);

        var mapped = expenses
            .Select(e => ExpenseResponse.FromEntity(e))
            .ToList();

        var totalCount = await _expenseRepository.CountByUserIdAsync(request.UserId, cancellationToken);

        var result = new PaginatedResult<ExpenseResponse>(
            mapped,
            totalCount,
            query.Page,
            query.PageSize);

        return Result.Success(result);
    }
}
