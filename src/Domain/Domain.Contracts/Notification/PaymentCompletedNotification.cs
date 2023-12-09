using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public record PaymentCompletedNotification :
    INotification, IPaymentCompleted
{
    public Guid IdNotification => Guid.NewGuid();   

    public Guid IdPayment { get; }

    public PaymentCompletedNotification(IPaymentCompleted payment)
    {        
        IdPayment = payment.IdPayment;  
    }

    public PaymentCompletedNotification()
    {
        
    }
}
