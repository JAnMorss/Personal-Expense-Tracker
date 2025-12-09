using SpendWise.Domain.Users.Entities;
using System.ComponentModel.DataAnnotations;

namespace SpendWise.Application.Users.Response;

public sealed class UserResponse
{
    public Guid Id { get; init; }

    public string? FullName { get; init; } = string.Empty;

    public int Age { get; init; }

    public string? Role { get; set; }

    [EmailAddress]
    public string? Email { get; init; }

    public string? Avatar { get; init; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public static UserResponse FromEntity(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = $"{user.FirstName.Value} {user.LastName.Value}",
            Age = user.Age.Value,   
            Role = user.Roles.FirstOrDefault()?.Name,
            Email = user.Email?.Value,
            Avatar = user.Avatar?.Value,
            UpdatedAt = user.UpdateAt
        };
    }

}
