using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.Application.Users.Queries.GetMyProfile;
using SpendWise.Application.Users.Queries.GetUserAvatar;
using SpendWise.Application.Users.Response;
using SpendWise.Server.Abstractions;

namespace SpendWise.Server.Controllers.Users;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/user")]
[Authorize]
public class UserController : ApiController
{
    public UserController(ISender sender) 
        : base(sender)
    {
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetMyProfileQuery(userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<UserResponse>(
                result.Value,
                "User profile retrieved successfully."))
            : HandleFailure(result);
    }

    [HttpGet("avatar")]
    public async Task<IActionResult> GetUserAvatar(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetUserAvatarQuery(userId.Value);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<UserAvatarResponse>(
                result.Value,
                "User avatar retrieved successfully."))
            : HandleFailure(result);
    }
}
