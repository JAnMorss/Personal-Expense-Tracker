using SpendWise.Application.Categories.Response;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Application.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(
    QueryObject? Query,
    Guid userId) : IQuery<PaginatedResult<CategoryResponse>>;