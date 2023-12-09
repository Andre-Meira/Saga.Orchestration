using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(IPaymentFailed))]
public interface IPaymentFailed 
{
    Guid IdPayment { get; set; }
    string Message { get; set; }
}
