using SpendWise.Application.Users.Response;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Query;

namespace SpendWise.Application.Users.Queries.GetMyProfile;

public sealed class GetMyProfileQueryHandler : IQueryHandler<GetMyProfileQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetMyProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound);

        return Result.Success(UserResponse.FromEntity(user));
    }
}
