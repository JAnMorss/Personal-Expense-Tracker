using SpendWise.Application.Categories.Response;
using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, PaginatedResult<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<PaginatedResult<CategoryResponse>>> Handle(
        GetAllCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var query = request.Query ?? new QueryObject();

        var categories = await _categoryRepository.GetAllAsync(
            query, 
            request.userId, 
            cancellationToken
        );

        if (categories is null || !categories.Any())
            return Result.Failure<PaginatedResult<CategoryResponse>>(CategoryErrors.EmptyCategory);

        var mapped = categories
            .Select(category => CategoryResponse.FromEntity(category))
            .ToList();

        var totalCount = await _categoryRepository.CountByUserIdAsync(request.userId, cancellationToken);

        var result = new PaginatedResult<CategoryResponse>(
            mapped,
            totalCount,
            query.Page,
            query.PageSize);

        return Result.Success(result);
    }
}
