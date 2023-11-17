namespace Domain.Contracts.Payment;

public sealed record PaymentCommand(
    Guid User,
    string CodDocument,
    decimal Value,
    string CurrencyCode);
