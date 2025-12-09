using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.Application.Auth.Commands.Login;
using SpendWise.Application.Auth.Commands.Register;
using SpendWise.Application.Auth.Response;
using SpendWise.Application.Users.Response;
using SpendWise.Domain.Users.Entities;
using SpendWise.Server.Abstractions;
using SpendWise.Server.Controllers.Auth.Requests;

namespace SpendWise.Server.Controllers.Auth;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ApiController
{
    public AuthController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Age,
            request.Email,
            request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<UserResponse>(
                result.Value,
                "User registered successfully."))
            : HandleFailure(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<AuthResponse>(
                result.Value,
                "Login successfully"))
            : HandleFailure(result);
    }
}
