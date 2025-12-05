using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Domain.Users.Interface;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
}
