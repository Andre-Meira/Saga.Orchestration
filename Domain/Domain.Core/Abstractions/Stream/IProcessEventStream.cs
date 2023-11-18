namespace Domain.Core.Abstractions.Stream;

public interface IProcessEventStream<T>
{
    Task Include(IEventStream @event);

    Task<T> Process(Guid Id);

    IEnumerable<IEventStream> GetEvents(Guid Id);
}
