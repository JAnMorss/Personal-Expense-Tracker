using SpendWise.SharedKernel.Domain.Entities;

namespace SpendWise.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{
    public static T AssertDomainEventWasPublished<T>(BaseEntity entity)
        where T : IDomainEvent
    {
        var domainEvents = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

        if (domainEvents is null)
        {
            throw new Exception($"{typeof(T).Name} was not published");
        }

        entity.ClearDomainEvents();

        return domainEvents;
    }
}
