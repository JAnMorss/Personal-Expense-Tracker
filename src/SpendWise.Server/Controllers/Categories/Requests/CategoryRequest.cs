namespace SpendWise.Server.Controllers.Categories.Requests;

public sealed record CategoryRequest(
    string CategoryName,
    string? Icon);
