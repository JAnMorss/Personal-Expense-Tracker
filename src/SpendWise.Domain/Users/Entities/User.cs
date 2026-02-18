using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Events;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Users.Entities;

public sealed class User : BaseEntity
{
    private readonly List<RefreshToken> _refreshTokens = new();
    private readonly List<Role> _roles = new();

    private User() { }

    public User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Age? age,
        Email email,
        PasswordHash passwordHash,
        Avatar? avatar) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
        PasswordHash = passwordHash;
        Avatar = avatar;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = null;
    }

    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Avatar? Avatar { get; private set; }
    public Age? Age { get; private set; }
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdateAt { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens 
        => _refreshTokens.AsReadOnly();
    public IReadOnlyCollection<Role> Roles 
        => _roles.ToList();

    public static Result<User> Create(
    Guid id,
    string firstName,
    string lastName,
    int age,
    string email,
    string? avatar,
    string passwordHash)
    {
        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure) 
            return Result.Failure<User>(firstNameResult.Error);

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure) 
            return Result.Failure<User>(lastNameResult.Error);

        var ageResult = Age.Create(age);
        if (ageResult.IsFailure) 
            return Result.Failure<User>(ageResult.Error);

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) 
            return Result.Failure<User>(emailResult.Error);

        var passwordHashResult = PasswordHash.Create(passwordHash);
        if (passwordHashResult.IsFailure) 
            return Result.Failure<User>(passwordHashResult.Error);

        Avatar? avatarValue = null;
        if (!string.IsNullOrWhiteSpace(avatar))
        {
            var avatarResult = Avatar.Create(avatar);
            if (avatarResult.IsFailure) 
                return Result.Failure<User>(avatarResult.Error);

            avatarValue = avatarResult.Value;
        }

        var user = new User(
            id,
            firstNameResult.Value,
            lastNameResult.Value,
            ageResult.Value,
            emailResult.Value,
            passwordHashResult.Value,
            avatarValue
        );

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return Result.Success(user);
    }

    public Result<User> UpdateDetails(string firstName, string lastName, int age, string email)
    {
        bool changed = false;

        if (!string.IsNullOrWhiteSpace(firstName) && firstName != FirstName?.Value)
        {
            var firstNameResult = FirstName.Create(firstName);
            if (firstNameResult.IsFailure) 
                return Result.Failure<User>(firstNameResult.Error);

            FirstName = firstNameResult.Value;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(lastName) && lastName != LastName?.Value)
        {
            var lastNameResult = LastName.Create(lastName);
            if (lastNameResult.IsFailure) 
                return Result.Failure<User>(lastNameResult.Error);

            LastName = lastNameResult.Value;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(age.ToString()) && age != Age?.Value)
        {
            var ageResult = Age.Create(age);
            if (ageResult.IsFailure) 
                return Result.Failure<User>(ageResult.Error);

            Age = ageResult.Value;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && email != Email?.Value)
        {
            var emailResult = Email.Create(email);
            if (emailResult.IsFailure) 
                return Result.Failure<User>(emailResult.Error);

            Email = emailResult.Value;
            changed = true;
        }

        if (changed)
        {
            UpdateAt = DateTime.UtcNow;
            RaiseDomainEvent(new UserUpdatedDomainEvent(Id));
        }

        return Result.Success(this);
    }

    public Result UpdateAvatar(string avatarUrl)
    {
        var avatarResult = Avatar.Create(avatarUrl);
        if (avatarResult.IsFailure)
            return Result.Failure(avatarResult.Error);

        Avatar = avatarResult.Value;
        UpdateAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserAvatarUpdatedDomainEvent(Id));

        return Result.Success(avatarResult.Value);
    }

    public Result PromoteToAdmin()
    {
        if ( _roles.Any(r => r == Role.Admin)) 
            return Result.Failure(UserErrors.AlreadyAdmin);

        _roles.Add(Role.Admin);

        RaiseDomainEvent(new UserPromotedToAdminDomainEvent(Id));
        return Result.Success();
    }

    public Result DemoteFromAdmin()
    {
        var adminRole = _roles.FirstOrDefault(r => r == Role.Admin);

        if (adminRole is null) 
            return Result.Failure(UserErrors.NotAnAdmin);

        _roles.Remove(adminRole);

        RaiseDomainEvent(new UserDemotedFromAdminDomainEvent(Id));
        return Result.Success();
    }

    public void AddRefreshToken(RefreshToken refreshToken)
        => _refreshTokens.Add(refreshToken);
}
