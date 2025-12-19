using SpendWise.Application.Categories.Response;
using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository, 
        IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAndUserIdAsync(request.Id, request.UserId, cancellationToken);
        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<CategoryResponse>(UserErrors.NotFound);

        return CategoryResponse.FromEntity(category, user);
    }
}
