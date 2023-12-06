using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(OrderPayment))]
public record OrderPayment(
    Guid IdPayment,
    Guid Payeer,    
    decimal Value) : IContract;

