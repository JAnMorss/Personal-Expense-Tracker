using SpendWise.Application.Categories.Response;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(
    Guid Id,
    Guid UserId) : IQuery<CategoryResponse>;