using Domain.Core.Abstractions.Entity;

namespace Domain.Core;

public class DomainExceptions : Exception
{
    public List<Notification>? Messages { get; }

    public DomainExceptions() { }

    public DomainExceptions(string message) : base(message) { }

    public DomainExceptions(string message, Exception innerException)
        : base(message, innerException) { }

    public DomainExceptions(List<Notification> messages) : base() => Messages = messages;
}