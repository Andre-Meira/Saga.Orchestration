namespace Domain.Contracts.Notification;

public interface IPaymentNotification
{
    Task Initilized(IPaymentInitializedNotification notification);

    Task Completed(IPaymentCompletedNotification notification);

    Task Faulted(IPaymentFailedNotification notification);
}
