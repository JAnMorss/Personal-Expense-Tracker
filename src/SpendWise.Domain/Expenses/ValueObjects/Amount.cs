using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Expenses.ValueObjects;

public sealed class Amount : ValueObject
{
    public decimal Value { get; }
    public const decimal MinValue = 0.01m;
    public const decimal MaxValue = 100_000m;

    public Amount(decimal value)
    {
        Value = value;
    }

    public static Result<Amount> Create(decimal amount)
    {
        if (amount < MinValue)
        {
            return Result.Failure<Amount>(new Error(
                "Amount.TooSmall",
                $"Amount must be at least {MinValue}."));
        }

        if (amount > MaxValue)
        {
            return Result.Failure<Amount>(new Error(
                "Amount.TooLarge",
                $"Amount must not exceed {MaxValue}."));
        }

        return new Amount(amount);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator decimal(Amount amount) 
        => amount.Value;
}
