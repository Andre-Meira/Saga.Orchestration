namespace Domain.Contracts.Payment;

public sealed record PaymentCommand(
    Guid IdPayment,
    Guid Payer,
    Guid Payee,    
    decimal Value);
