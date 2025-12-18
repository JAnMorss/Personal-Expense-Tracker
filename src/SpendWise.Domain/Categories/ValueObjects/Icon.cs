using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;
using System.Globalization;

namespace SpendWise.Domain.Categories.ValueObjects;

public sealed class Icon : ValueObject
{
    public string Value { get; }

    private Icon(string value)
    {
        Value = value;
    }

    public static Result<Icon> Create(string icon)
    {
        if (string.IsNullOrWhiteSpace(icon))
        {
            return Result.Failure<Icon>(new Error(
                "Icon.Empty",
                "Icon cannot be empty."));
        }

        var textElements = StringInfo.ParseCombiningCharacters(icon);

        if (textElements.Length == 0)
        {
            return Result.Failure<Icon>(new Error(
                "Icon.InvalidFormat",
                "Icon must be a valid emoji."));
        }

        if (textElements.Length > 1)
        {
            return Result.Failure<Icon>(new Error(
                "Icon.TooMany",
                "Icon must contain exactly one emoji."));
        }

        return Result.Success(new Icon(icon));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}