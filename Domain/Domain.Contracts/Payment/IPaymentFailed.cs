namespace Domain.Contracts.Payment;

internal interface IPaymentFailed
{
    Guid Id { get; }
    Guid IdPayment { get; }
    string Mensagem { get; }
}
