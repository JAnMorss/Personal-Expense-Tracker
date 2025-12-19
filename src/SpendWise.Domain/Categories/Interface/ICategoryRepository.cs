using SpendWise.Domain.Categories.Entities;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Domain.Categories.Interface;

public interface ICategoryRepository : IUserOwnedRepository<Category>
{
}
