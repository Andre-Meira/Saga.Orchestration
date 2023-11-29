using Domain.Core.Abstractions.Entity;

namespace Domain.Core;

public class DomainException : Exception
{
    public List<Notification>? Messages { get; }

    public DomainException() { }

    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }

    public DomainException(List<Notification> messages) : base() => Messages = messages;
}