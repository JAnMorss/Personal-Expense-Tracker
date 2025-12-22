using Microsoft.EntityFrameworkCore;
using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Expenses.Entities;
using SpendWise.Domain.Expenses.Interface;
using SpendWise.Infrastructure.Repositories.Base;
using SpendWise.SharedKernel.Helpers;

namespace SpendWise.Infrastructure.Repositories;

internal sealed class ExpenseRepository : UserOwnedRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public override async Task<IEnumerable<Expense>> GetAllAsync(
        QueryObject queryObject,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(_context, queryObject);
        query = ApplyFilters(query, queryObject, userId);
        query = ApplySorting(query, queryObject);

        int skip = (queryObject.Page - 1) * queryObject.PageSize;

        return await query
            .Include(e => e.Category)
            .Include(e => e.User)
            .Skip(skip)
            .Take(queryObject.PageSize)
            .ToListAsync(cancellationToken);

    }

    public async Task<Expense?> GetByIdAndUserIdAsync(
       Guid id,
       Guid userId,
       CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId, cancellationToken);
    }

    protected override IQueryable<Expense> ApplyFilters(
        IQueryable<Expense> query,
        QueryObject queryObject,
        Guid? userId = null)
    {
        if (!string.IsNullOrWhiteSpace(queryObject.Search))
        {
            if (decimal.TryParse(queryObject.Search, out var amountValue))
            {
                query = query.Where(c => c.Amount.Value == amountValue);
            }
        }

        if (userId.HasValue)
        {
            query = query.Where(c => c.CreatedByUserId == userId.Value);
        }

        return query;
    }

    protected override IQueryable<Expense> ApplySorting(
        IQueryable<Expense> query,
        QueryObject queryObject)
    {
        if (string.IsNullOrWhiteSpace(queryObject.SortBy))
            return query;

        query = queryObject.SortBy?.ToLower() switch
        {
            "amount" => queryObject.Descending
                ? query.OrderByDescending(c => c.Amount.Value)
                : query.OrderBy(c => c.Amount.Value),

            "createdat" => queryObject.Descending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt),

            _ => query
        };

        return query;
    }
}