using MassTransit;

namespace Domain.Contracts.Payment;


[EntityName(nameof(PaymentCommand))]
public sealed record PaymentCommand(
    Guid IdPayment,
    Guid Payer,
    Guid Payee,
    decimal Value) : IContract;
