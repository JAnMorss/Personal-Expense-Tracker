using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Users.Entities;

public sealed class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Token { get; private set; } = null!;

    public DateTime ExpiryDate { get; private set; }

    public bool IsRevoked { get; private set; }
    
    public User User { get; private set; } = null!;

    public bool IsExpired()
        => DateTime.UtcNow >= ExpiryDate;

    public void Revoke()
        => IsRevoked = true;
}
