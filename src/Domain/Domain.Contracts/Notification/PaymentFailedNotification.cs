using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public record PaymentFailedNotification :
    INotification, IPaymentFailed
{
    public Guid IdNotification => Guid.NewGuid();

    public Guid IdPayment { get; set; }

    public string Message { get; set; } = string.Empty;

    public PaymentFailedNotification(IPaymentFailed payment)
    {        
        IdPayment = payment.IdPayment;  
        Message = payment.Message;
    }

    public PaymentFailedNotification()
    {
        
    }
}
