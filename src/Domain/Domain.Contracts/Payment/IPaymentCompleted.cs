using MassTransit;

namespace Domain.Contracts.Payment;


public interface IPaymentCompleted 
{
    Guid IdPayment { get; }
}
