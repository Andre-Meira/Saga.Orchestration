using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public interface IPaymentFailedNotification : 
    INotification, IPaymentFailed
{ }
