using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public interface IPaymentInitializedNotification 
    : INotification, IPaymentInitialized
{ }
