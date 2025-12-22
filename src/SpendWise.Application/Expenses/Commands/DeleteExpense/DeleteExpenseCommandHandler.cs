using SpendWise.Domain.Expenses.Errors;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.DeleteExpense;

public sealed class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Guid>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAndUserIdAsync(
            request.Id,
            request.UserId, 
            cancellationToken);
        if (expense is null)
            return Result.Failure<Guid>(ExpenseErrors.NotFound);

        await _expenseRepository.DeleteAsync(expense.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(expense.Id);
    }
}
