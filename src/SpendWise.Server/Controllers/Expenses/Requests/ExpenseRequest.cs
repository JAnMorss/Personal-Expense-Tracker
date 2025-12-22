namespace SpendWise.Server.Controllers.Expenses.Requests;

public sealed record ExpenseRequest(
    decimal Amount,
    DateTime Date,
    string? Description);