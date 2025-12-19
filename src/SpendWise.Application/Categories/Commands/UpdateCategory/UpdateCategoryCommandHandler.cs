using SpendWise.Application.Categories.Response;
using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAndUserIdAsync(
            request.Id,
            request.UserId, 
            cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<CategoryResponse>(UserErrors.NotFound);

        var updateResult = category.UpdateCategory(
            request.CategoryName, 
            request.Icon);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CategoryResponse.FromEntity(category, user);
    }
}
