namespace Domain.Core.Abstractions.Stream;


/// <summary>
///     Clase base de eventos, todos os eventos do 
    /// sistema devera abstrair dessa interface
/// </summary>
public interface IEventStream
{
    public Guid IdCorrelation { get; init; }   
    public DateTime DataProcessed { get; init; }    
}
