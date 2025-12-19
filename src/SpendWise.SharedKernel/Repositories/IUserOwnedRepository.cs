using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.SharedKernel.Repositories;

public interface IUserOwnedRepository<T> : IRepository<T> where T : BaseEntity, IUserOwned
{
    Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
