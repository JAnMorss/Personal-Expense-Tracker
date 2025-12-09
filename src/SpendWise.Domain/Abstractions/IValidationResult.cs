using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Abstractions;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occured");

    Error[] Errors { get; }
}
