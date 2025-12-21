using SpendWise.Domain.Expenses.Entities;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.Infrastructure.Repositories.Base;

namespace SpendWise.Infrastructure.Repositories;

internal sealed class ExpenseRepository : UserOwnedRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
