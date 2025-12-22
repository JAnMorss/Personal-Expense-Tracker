using SpendWise.Application.Expenses.Response;
using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Application.Categories.Response;

public sealed class CategoryResponse
{
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Icon { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string? CreatedByUser { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public object Expenses { get; set; } = new List<ExpenseResponse>();

    public static CategoryResponse FromEntity(Category category)
    {
        var response = new CategoryResponse
        {
            Id = category.Id,
            CategoryName = category.CategoryName.Value,
            Icon = category.Icon?.Value,
            CreatedByUserId = category.CreatedByUserId,
            CreatedByUser = category.User is not null
                            ? $"{category.User.FirstName.Value} {category.User.LastName.Value}"
                            : null,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };

        response.Expenses = category.Expenses is null || !category.Expenses.Any()
            ? "No expenses yet. Please add an expense."
            : category.Expenses
                      .Select(ExpenseResponse.FromEntity)
                      .ToList();

        return response;
    }
}
