namespace Payment.Application.Machine.Activitys.BankActivity;

internal sealed record BankPaymentRequest
{
    public BankPaymentRequest(Guid payeer, decimal value)
    {
        Payeer = payeer;
        Value = value;
    }
    public Guid Payeer { get; init; }
    public decimal Value { get; init; }
}
