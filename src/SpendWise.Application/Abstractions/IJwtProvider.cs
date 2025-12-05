using SpendWise.Domain.Users.Entities;

namespace SpendWise.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);

    string GenerateRefreshToken();
}
