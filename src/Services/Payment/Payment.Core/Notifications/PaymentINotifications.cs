using Domain.Contracts.Notification;
using Domain.Contracts.Payment;

namespace Payment.Core.Notifications;

public record PaymentInitializedNotification : IPaymentInitializedNotification
{    
    public PaymentInitializedNotification(IPaymentInitialized paymentInitialized)
    {
        IdNotification = new Guid();
        IdPayment = paymentInitialized.IdPayment;
        Payee = paymentInitialized.Payee;
        Payeer = paymentInitialized.Payeer;
        Value = paymentInitialized.Value;
    }    

    public Guid IdNotification { get; set; }
    public Guid IdPayment { get ; set ; }
    public Guid Payeer { get ; set ; }
    public Guid Payee { get ; set ; }
    public decimal Value { get ; set ; }
}

public record PaymentCompletedNotification : IPaymentCompletedNotification
{
    public PaymentCompletedNotification(IPaymentCompleted payment)
    {
        IdNotification = new Guid();
        IdPayment = payment.IdPayment;        
    }

    public Guid IdNotification { get; set; }
    public Guid IdPayment { get; set; }    
}


public record PaymentFailedNotification : IPaymentFailedNotification
{
    public PaymentFailedNotification(IPaymentFailed payment)
    {
        IdNotification = new Guid();
        IdPayment = payment.IdPayment;
        Message = payment.Message;
    }

    public Guid IdNotification { get; set; }
    public Guid IdPayment { get; set; }
    public string Message { get; }
}