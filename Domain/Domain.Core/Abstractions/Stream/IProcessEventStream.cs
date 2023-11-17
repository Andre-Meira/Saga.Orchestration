namespace Domain.Core.Abstractions.Stream;

public interface IProcessEventStream<T>
{    
    Task Process(IEventStream @event);

    IEnumerable<T> GetEvents(Guid Id);
}
