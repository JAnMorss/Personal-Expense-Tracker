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

    public override async Task<User?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public override async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        foreach (var role in user.Roles)
        {
            _context.Attach(role);
        }

        await _context.AddAsync(user, cancellationToken);
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
