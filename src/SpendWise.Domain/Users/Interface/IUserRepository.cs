using SpendWise.Domain.Users.Entities;
using SpendWise.SharedKernel.Domain.Repositories;

namespace SpendWise.Domain.Users.Interface;

public interface IUserRepository : IRepository<User>
{
}
