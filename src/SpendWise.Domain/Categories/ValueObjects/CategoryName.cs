using SpendWise.SharedKernel.Domain.Entities;
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Categories.ValueObjects;

public sealed class CategoryName : ValueObject
{
    public string Value { get; }
    public const int MaxLength = 40;

    public CategoryName(string value)
    {
        Value = value;
    }

    public static Result<CategoryName> Create(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            return Result.Failure<CategoryName>(new Error(
                "categoryName.Empty",
                "Category Name cannot be empty."));
        }

        if (categoryName.Length > MaxLength)
        {
            return Result.Failure<CategoryName>(new Error(
                "categoryName.TooLong",
                $"Category Name cannot exceed {MaxLength} characters."));
        }

        return new CategoryName(categoryName);
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
