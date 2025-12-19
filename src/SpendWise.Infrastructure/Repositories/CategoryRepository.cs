using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Infrastructure.Repositories.Base;
using SpendWise.SharedKernel.Helpers;

namespace SpendWise.Infrastructure.Repositories;

internal sealed class CategoryRepository : UserOwnedRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) 
        : base(context)
    {
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
