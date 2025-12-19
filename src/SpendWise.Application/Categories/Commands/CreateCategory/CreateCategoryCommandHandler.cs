using SpendWise.Application.Categories.Response;
using SpendWise.Domain.Categories.Entities;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository, 
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<CategoryResponse>(UserErrors.NotFound);

        var categoryResult = Category.Create(
            Guid.NewGuid(),
            request.CategoryName,
            request.Icon ?? string.Empty,
            request.UserId);

        if (categoryResult.IsFailure)
            return Result.Failure<CategoryResponse>(categoryResult.Error);

        var category = categoryResult.Value;

        await _categoryRepository.AddAsync(category, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(CategoryResponse.FromEntity(category, user));
    }
}
