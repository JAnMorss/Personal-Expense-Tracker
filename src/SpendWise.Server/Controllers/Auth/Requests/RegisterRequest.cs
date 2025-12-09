namespace SpendWise.Server.Controllers.Auth.Requests;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    int Age,
    string Email,
    string Password);