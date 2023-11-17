namespace Domain.Core.Abstractions.Stream;

public interface IEventStream
{
    private object obj => this;

    Guid Guid => Guid.NewGuid();
    DateTime DataProcessed => DateTime.Now;
    string Name => nameof(obj);
}
