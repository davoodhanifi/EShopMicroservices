using MediatR;

namespace Ordering.Domain.Abstractions;
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn { get; }
    public string? EventType => GetType().AssemblyQualifiedName;
}
