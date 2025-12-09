namespace SpendWise.Server.Controllers.Auth.Requests;

public record LoginRequest(
    string Email,
    string Password);
