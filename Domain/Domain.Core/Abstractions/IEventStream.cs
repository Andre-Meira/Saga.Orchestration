namespace Domain.Core.Abstractions;

public interface IEventStream
{
    private object obj => this;

    Guid Guid { get; }

    string Name => nameof(obj);

    DateTime date => DateTime.Now;    
}
