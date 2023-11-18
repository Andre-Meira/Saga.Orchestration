using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(IPaymentFailed))]
public interface IPaymentFailed : IContract
{
    Guid IdPayment { get; }
    string Mensagem { get; }
}
