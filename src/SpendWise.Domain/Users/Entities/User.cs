using SpendWise.Domain.Users.Events;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Users.Entities;

public sealed class User : BaseEntity
{
    private readonly List<RefreshToken> _refreshTokens = new();

    private User()
    {
        
    }
    public User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Age age,
        Email email,
        PasswordHash passwordHash,
        Avatar? avatar,
        Role role) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
        PasswordHash = passwordHash;
        Avatar = avatar;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = null;
    }

    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Avatar? Avatar { get; private set; }
    public Age Age { get; private set; }
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdateAt { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens 
        => _refreshTokens.AsReadOnly();

    public Result Create(
        Guid id,
        string firstName,
        string lastName,
        int age,
        string emai,
        string? avatar,
        string passwordHash)
    {
        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
            return Result.Failure(firstNameResult.Error);

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
            return Result.Failure(lastNameResult.Error);

        var ageResult = Age.Create(age);
        if (ageResult.IsFailure)
            return Result.Failure(ageResult.Error);

        var emailResult = Email.Create(emai);
        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        Avatar? avatarValue = null;
        if (!string.IsNullOrWhiteSpace(avatar))
        {
            var avatarResult = Avatar.Create(avatar!);
            if (avatarResult.IsFailure)
                return Result.Failure(avatarResult.Error);

            avatarValue = avatarResult.Value;
        }

        var passwordHashResult = PasswordHash.Create(passwordHash);
        if (passwordHashResult.IsFailure)
            return Result.Failure(passwordHashResult.Error);

        var user = new User(
            id,
            firstNameResult.Value,
            lastNameResult.Value,
            ageResult.Value,
            emailResult.Value,
            passwordHashResult.Value,
            avatarValue,
            Role.Registered);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return Result.Success(user);
    }
}
