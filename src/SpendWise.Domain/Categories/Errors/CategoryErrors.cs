
using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Categories.Errors;

public static class CategoryErrors
{
    public static readonly Error NotFound = new(
        "Category.NotFound",
        "The specified category was not found.");

    public static readonly Error EmptyCategory = new(
        "Category.Empty",
        "Your category list is empty. Please create a category first");
}
