using SpendWise.Domain.Categories.Events;
using SpendWise.Domain.Categories.ValueObjects;
using SpendWise.Domain.Expenses.Entities;
using SpendWise.Domain.Expenses.Errors;
using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Domain.Categories.Entities;

public sealed class Category : BaseEntity, IUserOwned
{
    private readonly List<Expense> _expenses = new();

    private Category() { }

    public Category(
        Guid Id,
        CategoryName categoryName,
        Icon? icon,
        Guid createdByUserId) : base(Id)
    {
        CategoryName = categoryName;
        Icon = icon;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public CategoryName CategoryName { get; private set; } = null!;

    public Icon? Icon { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    public static Result<Category> Create(
        Guid id,
        string categoryName,
        string icon,
        Guid createdByUserId)
    {
        var categoryNameResult = ResultHelper.CreateOrFail(CategoryName.Create, categoryName);

        Icon? iconValue = null;
        if (!string.IsNullOrWhiteSpace(icon))
        {
            iconValue = ResultHelper.CreateOrFail(Icon.Create, icon);
        }

        var category = new Category(
            id,
            categoryNameResult,
            iconValue,
            createdByUserId);

        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return Result.Success(category);
    }

    public Result UpdateCategory(
        string categoryName,
        string? icon)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(categoryName) 
            && categoryName != CategoryName.Value)
        {
            CategoryName = ResultHelper.CreateOrFail(CategoryName.Create, categoryName);
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(icon) 
            && icon != Icon?.Value)
        {
            Icon = ResultHelper.CreateOrFail(Icon.Create, icon);
            isUpdated = true;
        }

        if (isUpdated)
        {
            UpdatedAt = DateTime.UtcNow;
            RaiseDomainEvent(new CategoryUpdatedDomainEvent(Id));
        }

        return Result.Success(this);
    }

    public Result AddExpense(Expense expense)
    {
        if (expense.CategoryId != Id)
            return Result.Failure<Expense>(ExpenseErrors.InvalidCategoryId);

        _expenses.Add(expense);

        RaiseDomainEvent(new ExpenseAddedToCategoryDomainEvent(Id, expense.Id));

        return Result.Success(this);
    }
}
