using MassTransit;

namespace Domain.Contracts.Payment;

[EntityName(nameof(ProcessPayment))]
public record ProcessPayment(
    Guid IdPayment,
    Guid Payeer,    
    decimal Value) : IContract;

