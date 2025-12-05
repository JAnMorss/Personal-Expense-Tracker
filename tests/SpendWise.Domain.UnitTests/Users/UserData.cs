using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.ValueObjects;

namespace SpendWise.Domain.UnitTests.Users;

internal static class UserData
{
    public static User Create() => User.Create(
        Id,
        FirstName.Value,
        LastName.Value,
        Age.Value,
        Email.Value,
        Avatar.Value,
        PasswordHash.Value).Value;

    public static readonly Guid Id = 
        Guid.NewGuid();

    public static readonly FirstName FirstName =
        FirstName.Create("John Anthony").Value;

    public static readonly LastName LastName =
        LastName.Create("Morales").Value;

    public static readonly Avatar Avatar =
        Avatar.Create("https://example.com/avatar.jpg").Value;

    public static readonly Age Age = 
        Age.Create(25).Value;

    public static readonly Email Email =
        Email.Create("Janmors13@gmail.com").Value;

    public static readonly PasswordHash PasswordHash =
        PasswordHash.Create("janmors123").Value;

}
