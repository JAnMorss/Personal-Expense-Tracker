using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Expenses.Events;

public sealed record ExpenseCreatedDomainEvent(Guid Id) : IDomainEvent;
