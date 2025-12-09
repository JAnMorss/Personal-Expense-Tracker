using SpendWise.Application.Abstractions;
using SpendWise.Application.Users.Response;
using SpendWise.Domain.Users.Entities;
using SpendWise.Domain.Users.Interface;
using SpendWise.Domain.Users.ValueObjects;
using SpendWise.SharedKernel;
using SpendWise.SharedKernel.ErrorHandling;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler 
    : ICommandHandler<RegisterCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository, 
        IJwtProvider jwtProvider, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserResponse>> Handle(
        RegisterCommand request, 
        CancellationToken cancellationToken)
    {
        var passwordHash = PasswordHash.FromPlainText(request.Password);

        var userResult = User.Create(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            request.Age,
            request.Email,
            null,
            passwordHash.Value);

        if (userResult.IsFailure)
            return Result.Failure<UserResponse>(userResult.Error);

        var user = userResult.Value;

        var token = _jwtProvider.Generate(user);

        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(UserResponse.FromEntity(user));
    }
}
