using SpendWise.Application.Users.Response;
using SpendWise.SharedKernel.Mediator.Command;

namespace SpendWise.Application.Auth.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    int Age,
    string Email,
    string Password) : ICommand<UserResponse>;