using Microsoft.EntityFrameworkCore;
using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Infrastructure.Repositories.Base;

internal abstract class UserOwnedRepository<T> : Repository<T>, IUserOwnedRepository<T>
    where T : BaseEntity, IUserOwned
{
    protected UserOwnedRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<int> CountByUserIdAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Set<T>()
            .CountAsync(e => e.CreatedByUserId == userId, cancellationToken);
    }

    public async Task<T?> GetByIdAndUserIdAsync(
        Guid id, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId, cancellationToken);
    }
}
