namespace Domain.Core.Abstractions;

public interface IAggregateStream
{    
    Guid Guid { get; }

    void When(IEventStream @event);
}
