using Domain.Contracts.Payment;

namespace Domain.Contracts.Notification;

public record PaymentInitializedNotification
    : INotification, IPaymentInitialized
{
    public Guid IdNotification => Guid.NewGuid();
    public Guid IdPayment { get ; set ; }
    public Guid Payeer { get ; set ; }
    public Guid Payee { get ; set ; }
    public decimal Value { get ; set ; }

    public PaymentInitializedNotification(IPaymentInitialized payment)
    {        
        IdPayment = payment.IdPayment;
        Payeer = payment.Payee;
        Payee = payment.Payee;
        Value = payment.Value;
    }

    public PaymentInitializedNotification()
    {

    }
}
