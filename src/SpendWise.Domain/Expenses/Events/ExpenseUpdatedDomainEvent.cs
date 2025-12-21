using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Expenses.Events;

public sealed record ExpenseUpdatedDomainEvent(Guid Id) : IDomainEvent;
