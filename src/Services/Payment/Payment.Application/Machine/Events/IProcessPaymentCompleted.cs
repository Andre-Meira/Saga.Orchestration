namespace Payment.Application.Machine.Events;

public interface IProcessPaymentCompleted
{
    Guid IdPayment { get; }    
}
