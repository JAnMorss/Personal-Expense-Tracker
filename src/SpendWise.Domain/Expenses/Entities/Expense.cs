

using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Expenses.Events;
using SpendWise.Domain.Expenses.ValueObjects;
using SpendWise.Domain.Users.Entities;
using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Domain.Expenses.Entities;

public class Expense : BaseEntity, IUserOwned
{
    private Expense() { }

    public Expense(
        Guid id,
        Amount amount,
        Guid categoryId,
        DateTime date,
        Description? description,
        Guid createdByUserId) : base(id)
    {
        Amount = amount;
        CategoryId = categoryId;
        Date = date;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Amount Amount { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public DateTime Date { get; private set; }
    public Description? Description { get; private set; } 
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set;  } 
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public Category? Category { get; private set; }
    public User? User { get; private set; }


    public static Result<Expense> Create(
        decimal amount,
        Guid categoryId,
        DateTime date,
        string? description,
        Guid createdByUserId)
    {
        var amountResult = ResultHelper.CreateOrFail(Amount.Create, amount);
        var descriptionResult = ResultHelper.CreateOrFail(Description.Create, description ?? string.Empty);

        var expense = new Expense(
            Guid.NewGuid(),
            amountResult,
            categoryId,
            date,
            descriptionResult,
            createdByUserId);

        expense.RaiseDomainEvent(new ExpenseCreatedDomainEvent(expense.Id));

        return Result.Success(expense);
    }

    public Result UpdateExpense(
        decimal amount,
        DateTime date,
        string? description)
    {
        bool isUpdated = false;

        if (amount != Amount.Value)
        {
            Amount = ResultHelper.CreateOrFail(Amount.Create, amount);
            isUpdated = true;
        }

        if (date != Date)
        {
            Date = date;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) 
            && description != Description?.Value)
        {
            Description = ResultHelper.CreateOrFail(Description.Create, description);
            isUpdated = true;
        }

        if (isUpdated)
        {
            UpdatedAt = DateTime.UtcNow;    
            RaiseDomainEvent(new ExpenseUpdatedDomainEvent(Id));
        }

        return Result.Success(this);
    }

}
