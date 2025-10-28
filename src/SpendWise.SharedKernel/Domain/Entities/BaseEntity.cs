namespace SpendWise.SharedKernel.Domain.Entities
{
    public abstract class BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; init; }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.ToList();
        }

        public void ClearDomainEvent()
            => _domainEvents.Clear();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);
    }
}
