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
        Age age,
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
    public Age Age { get; private set; }
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public Role Role { get; private set; }
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
        var firstNameResult = ResultHelper.CreateOrFail(FirstName.Create, firstName);
        var lastNameResult = ResultHelper.CreateOrFail(LastName.Create, lastName);
        var ageResult = ResultHelper.CreateOrFail(Age.Create, age);
        var emailResult = ResultHelper.CreateOrFail(Email.Create, email);
        var passwordHashResult = ResultHelper.CreateOrFail(PasswordHash.Create, passwordHash);

        Avatar? avatarValue = null;
        if (!string.IsNullOrWhiteSpace(avatar))
        {
            var avatarResult = ResultHelper.CreateOrFail(Avatar.Create, avatar);

            avatarValue = avatarResult;
        }

        var user = new User(
            id,
            firstNameResult,
            lastNameResult,
            ageResult,
            emailResult,
            passwordHashResult,
            avatarValue);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return Result.Success(user);
     }

    public Result<User> UpdateDetails(
        string firstName,
        string lastName,
        int age,
        string email)
    {
        bool changed = false;

        if (!string.IsNullOrWhiteSpace(firstName) && firstName != FirstName?.Value)
        {
            var firstNameResult = ResultHelper.CreateOrFail(FirstName.Create, firstName);

            FirstName = firstNameResult;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(lastName) && lastName != LastName?.Value)
        {
            var lastNameResult = ResultHelper.CreateOrFail(LastName.Create, lastName);

            LastName = lastNameResult;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(age.ToString()) && age != Age?.Value)
        {
            var ageResult = ResultHelper.CreateOrFail(Age.Create, age);

            Age = ageResult;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && email != Email?.Value)
        {
            var emailResult = ResultHelper.CreateOrFail(Email.Create, email);

            Email = emailResult;
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
        var avatar = ResultHelper.CreateOrFail(Avatar.Create, avatarUrl);

        Avatar = avatar;
        UpdateAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserAvatarUpdatedDomainEvent(Id));

        return Result.Success(avatar);
    }

    public void AddRefreshToken(RefreshToken refreshToken)
        => _refreshTokens.Add(refreshToken);
}
