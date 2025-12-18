using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Users.Entities;

namespace SpendWise.Application.Categories.Response;

public sealed class CategoryResponse
{
    public Guid Id { get; init; }
    public string CategoryName { get; init; } = null!;
    public string? Icon { get; init; }
    public Guid CreatedByUserId { get; init; }
    public string? CreatedByUser { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static CategoryResponse FromEntity(Category category, User user)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            CategoryName = category.CategoryName.Value,
            Icon = category.Icon?.Value,
            CreatedByUserId = user.Id,
            CreatedByUser = user != null
                            ? $"{user.FirstName.Value} {user.LastName.Value}"
                            : null,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}
