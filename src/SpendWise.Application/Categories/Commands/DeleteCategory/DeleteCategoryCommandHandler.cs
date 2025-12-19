using SpendWise.Domain.Categories.Errors;
using SpendWise.Domain.Categories.Interface;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAndUserIdAsync(
            request.Id, 
            request.UserId, 
            cancellationToken);

        if (category is null)
            return Result.Failure<Guid>(CategoryErrors.NotFound);

        await _categoryRepository.DeleteAsync(category.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
