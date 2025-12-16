using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.Users.Events;

public sealed record UserDemotedFromAdminDomainEvent(Guid Id) : IDomainEvent;
