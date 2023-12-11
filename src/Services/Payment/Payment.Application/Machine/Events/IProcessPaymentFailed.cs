namespace Payment.Application.Machine.Events;

public interface IProcessPaymentFailed
{
    Guid IdPayment { get; }
    string Message { get; }
}
