using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(Guid Id) : IDomainEvent;
