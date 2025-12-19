using SpendWise.Application.Categories.Response;
using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Helpers;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.PageSize;

namespace SpendWise.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, PaginatedResult<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;

    public GetAllCategoriesQueryHandler(
        ICategoryRepository categoryRepository, 
        IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
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

        var user = await _userRepository.GetByIdAsync(request.userId, cancellationToken);
        if (user is null)
            return Result.Failure<PaginatedResult<CategoryResponse>>(UserErrors.NotFound);

        var mapped = categories
            .Select(category => CategoryResponse.FromEntity(category, user))
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
