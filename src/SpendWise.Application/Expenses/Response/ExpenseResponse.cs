using SpendWise.Domain.Expenses.Entities;

namespace SpendWise.Application.Expenses.Response;

public sealed class ExpenseResponse
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public Guid CreatedByUserId { get; set; }
    public string? CreatedBy { get; set; }

    public decimal Amount { get; set; } 
    public DateTime Date { get; set; }
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static ExpenseResponse FromEntity(Expense expense)
    {
        return new ExpenseResponse
        {
            Id = expense.Id,

            CategoryId = expense.CategoryId,
            CategoryName = expense.Category?.CategoryName.Value,

            CreatedByUserId = expense.CreatedByUserId,
            CreatedBy = expense.User is not null
                        ? $"{expense.User.FirstName.Value} {expense.User.LastName.Value}" 
                        : null,

            Amount = expense.Amount.Value,
            Date = expense.Date,
            Description = expense.Description?.Value,

            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
        };
    }
}
