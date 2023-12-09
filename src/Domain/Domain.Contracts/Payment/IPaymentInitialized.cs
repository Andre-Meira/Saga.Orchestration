using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(IPaymentInitialized))]
public interface IPaymentInitialized
{
    Guid IdPayment { get; set; }
    Guid Payeer { get; set; }
    Guid Payee { get; set; }    
    decimal Value { get; set; } 
}
