using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Categories.Events;

public sealed record ExpenseAddedToCategoryDomainEvent(
    Guid CategoryId,
    Guid ExpenseId) : IDomainEvent;