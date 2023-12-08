using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public interface IPaymentCompletedNotification : 
    INotification, IPaymentCompleted 
{ }
