using MassTransit;

namespace Domain.Contracts.Payment;


[EntityName(nameof(OrderPayment))]
public sealed record OrderPayment(
    Guid IdPayment,
    Guid Payeer,
    Guid Payee,
    decimal Value) : IContract;
