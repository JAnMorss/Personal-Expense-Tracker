using SpendWise.Application.Users.Response;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Query;
using SpendWise.SharedKernel.Storage;

namespace SpendWise.Application.Users.Queries.GetUserAvatar;

public sealed class GetUserAvatarQueryHandler : IQueryHandler<GetUserAvatarQuery, UserAvatarResponse>
{
    private readonly IUserRepository _userRepository; 
    private readonly IAvatarBlobService _avatarService;

    public GetUserAvatarQueryHandler(
        IUserRepository userRepository,
        IAvatarBlobService avatarService)
    {
        _userRepository = userRepository;
        _avatarService = avatarService;
    }

    public async Task<Result<UserAvatarResponse>> Handle(GetUserAvatarQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<UserAvatarResponse>(UserErrors.NotFound);

        if (user.Avatar is null || string.IsNullOrWhiteSpace(user.Avatar.Value))
            return Result.Failure<UserAvatarResponse>(UserErrors.AvatarNotFound);

        var fileId = Avatar.ExtractFileIdFromUrl(user.Avatar.Value);
        if (!fileId.HasValue)
            return Result.Failure<UserAvatarResponse>(UserErrors.AvatarInvalidUrl);

        var downloadResult = await _avatarService.DownloadAsync(fileId.Value, cancellationToken);
        if (downloadResult is null)
            return Result.Failure<UserAvatarResponse>(UserErrors.AvatarNotFound);

        using var memoryStream = new MemoryStream();
        await downloadResult.Stream.CopyToAsync(memoryStream, cancellationToken);

        var imageBytes = memoryStream.ToArray();

        var response = UserAvatarResponse.FromEntity(
            user,
            imageBytes,
            downloadResult.ContentType);

        return Result.Success(response);
    }
}
