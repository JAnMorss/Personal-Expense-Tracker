using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Expenses.ValueObjects;

public sealed class Description : ValueObject
{
    public string Value { get; }
    public const int MaxLength = 500;
    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Failure<Description>(new Error(
                "Description.Empty",
                "Description cannot be empty."));
        }

        if (description.Length > MaxLength)
        {
            return Result.Failure<Description>(new Error(
                "Description.TooLong",
                $"Description cannot exceed {MaxLength} characters."));
        }

        return new Description(description);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
