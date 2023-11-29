namespace Domain.Core.Abstractions.Entity;

internal class Entity : INotificationDomain
{
    private readonly List<Notification> _notifications = new List<Notification>();

    private Guid _id = Guid.Empty;

    public bool IsDeleted { get; private set; }

    public Guid Id { get; private set; }

    public void AddNotification(Notification notification)
    {
        throw new NotImplementedException();
    }

    public virtual void Create()
    {
        Validate();

        if (_id.Equals(Guid.Empty) == false)
            throw new DomainException("já existe um id cadastrado para essa entidade.");

        _id = Guid.NewGuid();
        Id = _id;
    }

    public void Remove() => IsDeleted = true;

    public virtual void Validate()
    {
        if (_notifications.Any() == true)
            throw new DomainException(_notifications);
    }
}
