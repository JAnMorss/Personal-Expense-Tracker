using Microsoft.EntityFrameworkCore;
using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.Interface;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.Infrastructure.Repositories.Base;

namespace SpendWise.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(
        Email email, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);
    }
}
