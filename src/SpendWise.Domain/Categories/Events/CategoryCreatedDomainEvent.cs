using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Categories.Events;

public sealed record CategoryCreatedDomainEvent(Guid Id) : IDomainEvent;
