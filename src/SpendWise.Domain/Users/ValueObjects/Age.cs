using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Users.ValueObjects;

public sealed class Age : ValueObject
{
    public int Value { get; }

    public Age(int value)
    {
        Value = value;
    }

    public static Result<Age> Create(int age)
    {
        if (age < 0 || age > 100)
        {
            return Result.Failure<Age>(new Error(
                "Age.Invalid",
                "Age must be between 0 and 100."));
        }

        return new Age(age);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString()
        => Value.ToString();
}
