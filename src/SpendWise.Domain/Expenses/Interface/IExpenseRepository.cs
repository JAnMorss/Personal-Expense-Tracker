using SpendWise.Domain.Expenses.Entities;
using SpendWise.SharedKernel.Repositories;

namespace SpendWise.Domain.Expenses.Interface;

public interface IExpenseRepository : IUserOwnedRepository<Expense>
{
}
