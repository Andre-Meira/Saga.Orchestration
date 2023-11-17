namespace Domain.Contracts.Payment;

internal interface IPaymentFailed
{    
    Guid IdPayment { get; }
    string Mensagem { get; }
}
