using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user was not found.");
}
