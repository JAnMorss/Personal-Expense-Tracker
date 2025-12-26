using SpendWise.SharedKernel.ErrorHandling;

namespace SpendWise.Domain.Expenses.Errors;

public static class ExpenseErrors
{
    public static readonly Error NotFound = new(
        "Task.NotFound",
        "The task with the specified identifier was not found.");

    public static readonly Error InvalidCategoryId = new(
        "Expense.InvalidCategoryId",
        "The specified category ID is invalid for this expense.");

    public static readonly Error EmptyExpense = new(
        "Expense.EmptyExpense",
        "Your expense list is empty. Please create a expense first.");
}
