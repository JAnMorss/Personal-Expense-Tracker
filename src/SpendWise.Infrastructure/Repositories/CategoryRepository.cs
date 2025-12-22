using Microsoft.EntityFrameworkCore;
using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Expenses.Entities;
using SpendWise.Infrastructure.Repositories.Base;
using SpendWise.SharedKernel.Helpers;

namespace SpendWise.Infrastructure.Repositories;

internal sealed class CategoryRepository : UserOwnedRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public override async Task<IEnumerable<Category>> GetAllAsync(
        QueryObject queryObject,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(_context, queryObject);
        query = ApplyFilters(query, queryObject, userId);
        query = ApplySorting(query, queryObject);

        int skip = (queryObject.Page - 1) * queryObject.PageSize;

        return await query
            .Include(e => e.Expenses)
            .Include(e => e.User)
            .Skip(skip)
            .Take(queryObject.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAndUserIdAsync(
       Guid id,
       Guid userId,
       CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(e => e.Expenses)
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId, cancellationToken);
    }

    protected override IQueryable<Category> ApplyFilters(
        IQueryable<Category> query,
        QueryObject queryObject,
        Guid? userId = null)
    {
        if (!string.IsNullOrWhiteSpace(queryObject.Search))
        {
            query = query.Where(c => c.CategoryName.Value.Contains(queryObject.Search));
        }

        if (userId.HasValue)
        {
            query = query.Where(c => c.CreatedByUserId == userId.Value);
        }

        return query;
    }

    protected override IQueryable<Category> ApplySorting(
        IQueryable<Category> query,
        QueryObject queryObject)
    {
        if (string.IsNullOrWhiteSpace(queryObject.SortBy))
            return query;

        query = queryObject.SortBy?.ToLower() switch
        {
            "categoryname" => queryObject.Descending
                ? query.OrderByDescending(c => c.CategoryName.Value)
                : query.OrderBy(c => c.CategoryName.Value),

            "createdat" => queryObject.Descending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt),

            _ => query
        };

        return query;
    }

}
