using SpendWise.Application.Auth.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<AuthResponse>;