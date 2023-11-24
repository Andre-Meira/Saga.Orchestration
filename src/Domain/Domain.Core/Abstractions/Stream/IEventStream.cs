namespace Domain.Core.Abstractions.Stream;

public interface IEventStream
{
    public Guid IdCorrelation { get; init; }   
    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
}
