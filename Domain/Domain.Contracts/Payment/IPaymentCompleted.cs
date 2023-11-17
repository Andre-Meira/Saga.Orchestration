namespace Domain.Contracts.Payment;

internal interface IPaymentCompleted
{
    Guid Id { get; }
    Guid IdPayment { get; }
}
