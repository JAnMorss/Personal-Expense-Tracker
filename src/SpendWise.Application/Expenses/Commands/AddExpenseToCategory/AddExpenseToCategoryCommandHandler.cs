using SpendWise.Application.Categories.Response;
using SpendWise.Application.Expenses.Response;
using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Expenses.Entities;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Expenses.Commands.AddExpenseToCategory;

public sealed class AddExpenseToCategoryCommandHandler : ICommandHandler<AddExpenseToCategoryCommand, ExpenseResponse>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExpenseToCategoryCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result<ExpenseResponse>> Handle(AddExpenseToCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAndUserIdAsync(
            request.CategoryId,
            request.UserId,
            cancellationToken);
        if (category is null)
            return Result.Failure<ExpenseResponse>(CategoryErrors.NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<ExpenseResponse>(UserErrors.NotFound);

        var expenseResult = Expense.Create(
            request.Amount,
            request.CategoryId,
            request.Date,
            request.Description,
            request.UserId);

        if (expenseResult.IsFailure)
            return Result.Failure<ExpenseResponse>(expenseResult.Error);

        var expense = expenseResult.Value;

        var addResult = category.AddExpense(expense);
        if (addResult.IsFailure)
            return Result.Failure<ExpenseResponse>(addResult.Error);

        await _expenseRepository.AddAsync(expense, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ExpenseResponse.FromEntity(expense));
    }
}
