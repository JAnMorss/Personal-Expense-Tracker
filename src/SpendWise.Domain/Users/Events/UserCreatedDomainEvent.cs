using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid Id) : IDomainEvent;