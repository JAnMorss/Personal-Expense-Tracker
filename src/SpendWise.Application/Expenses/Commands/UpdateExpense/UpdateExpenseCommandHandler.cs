using SpendWise.Application.Expenses.Response;
using SpendWise.Domain.Expenses.Errors;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.UpdateExpense;

public sealed class UpdateExpenseCommandHandler 
    : ICommandHandler<UpdateExpenseCommand, ExpenseResponse>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ExpenseResponse>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAndUserIdAsync(
            request.Id,
            request.UserId,
            cancellationToken);

        if(expense is null)
            return Result.Failure<ExpenseResponse>(ExpenseErrors.NotFound);
        
        var updateResult = expense.UpdateExpense(
            request.Amount,
            request.Date,
            request.Description);

        await _expenseRepository.UpdateAsync(expense, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ExpenseResponse.FromEntity(expense);
    }
}
