namespace Domain.Core.Abstractions.Stream;

public interface IAggregateStream
{
    void When(IEventStream @event);
}
