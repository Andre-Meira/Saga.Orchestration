using MassTransit;

namespace Domain.Contracts.Payment;


[EntityName(nameof(PaymentCommand))]
public sealed record PaymentCommand(
    Guid IdPayment,
    Guid Payeer,
    Guid Payee,
    decimal Value) : IContract;
