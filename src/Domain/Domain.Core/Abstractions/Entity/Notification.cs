namespace Domain.Core.Abstractions.Entity;

public interface INotificationDomain
{
    public void AddNotification(Notification notification);
}

public sealed record Notification(string key, string value);