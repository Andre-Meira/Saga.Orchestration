using MassTransit;

namespace Domain.Contracts.Payment;


[EntityName(nameof(IPaymentCompleted))]
public interface IPaymentCompleted : IContract
{
    Guid IdPayment { get; }
}
