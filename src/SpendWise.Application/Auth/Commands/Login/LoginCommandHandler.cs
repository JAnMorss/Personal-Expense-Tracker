using SpendWise.Application.Abstractions;
using SpendWise.Application.Auth.Response;
using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.Errors;
using SpendWise.Domain.Users.Interface;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;
using System.Net.Mail;

namespace SpendWise.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository, 
        IJwtProvider jwtProvider, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request, 
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<AuthResponse>(emailResult.Error);

        var user = await _userRepository.GetByEmailAsync(
            emailResult.Value,
            cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (!user.PasswordHash!.Verify(request.Password))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var token = _jwtProvider.Generate(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken(
            user.Id,
            refreshToken,
            DateTime.UtcNow.AddDays(7));

        user.AddRefreshToken(refreshTokenEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthResponse(token, refreshToken));
    }
}
