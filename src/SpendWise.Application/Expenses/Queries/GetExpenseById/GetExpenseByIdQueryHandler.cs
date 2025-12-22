using SpendWise.Application.Expenses.Response;
using SpendWise.Domain.Expenses.Errors;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Expenses.Queries.GetExpenseById;

public sealed class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, ExpenseResponse>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<ExpenseResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAndUserIdAsync(
            request.Id,
            request.UserId, 
            cancellationToken);

        if (expense is null)
            return Result.Failure<ExpenseResponse>(ExpenseErrors.NotFound);

        return ExpenseResponse.FromEntity(expense);
    }
}
