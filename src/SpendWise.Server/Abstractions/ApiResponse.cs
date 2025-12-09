namespace SpendWise.Server.Abstractions;

public record ApiResponse<T>(T Data, string? Message);
public record ApiResponse(string Message);
