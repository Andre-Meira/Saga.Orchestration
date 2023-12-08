using MassTransit;

namespace Domain.Contracts.Payment;

public interface IPaymentFailed 
{
    Guid IdPayment { get; }
    string Message { get; }
}
