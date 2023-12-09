using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(IPaymentCompleted))]
public interface IPaymentCompleted 
{
    Guid IdPayment { get; set; }
}
