namespace Consumer.Bank;

internal sealed class BankPaymentRequest
{
    public BankPaymentRequest(Guid payeer, decimal value)
    {
        Payeer = payeer;
        Value = value;
    }

    public Guid Payeer { get; init; }
    public decimal Value { get; init; }
}
