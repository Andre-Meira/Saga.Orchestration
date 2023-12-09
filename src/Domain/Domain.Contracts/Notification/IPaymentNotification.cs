namespace Domain.Contracts.Notification;

public interface IPaymentNotification
{
    Task Initilized(PaymentInitializedNotification notification);

    Task Completed(PaymentCompletedNotification notification);

    Task Faulted(PaymentFailedNotification notification);
}
