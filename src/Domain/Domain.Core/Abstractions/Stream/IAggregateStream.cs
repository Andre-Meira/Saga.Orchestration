namespace Domain.Core.Abstractions.Stream;

public interface IAggregateStream<EventStream> where EventStream : IEventStream
{
    void When(EventStream @event);
}
