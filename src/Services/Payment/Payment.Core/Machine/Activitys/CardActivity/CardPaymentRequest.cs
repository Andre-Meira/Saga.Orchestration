namespace Payment.Core.Machine.Activitys.CardActivity;

internal sealed record CardPaymentRequest
{
    public CardPaymentRequest(Guid payeer, decimal value)
    {
        Payeer = payeer;
        Value = value;
    }
    public Guid Payeer { get; init; }
    public decimal Value { get; init; }
}
