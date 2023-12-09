namespace Payment.Core.Machine.Events;

public interface IProcessPaymentCompleted
{
    Guid IdPayment { get; }    
}
