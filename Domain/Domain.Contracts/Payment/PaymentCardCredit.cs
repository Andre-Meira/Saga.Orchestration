namespace Domain.Contracts.Payment;

internal interface PaymentCard : IPayment
{
    string CardId { get; init; }

    long Number { get; init; }

    int Code { get; init; }
}
