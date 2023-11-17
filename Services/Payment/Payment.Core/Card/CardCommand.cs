namespace Payment.Core.Bank;

internal sealed record BankCommand
{
    public BankCommand(
        Guid user, 
        decimal value, 
        string currencyCode)
    {
        User = user;
        Value = value;
        CurrencyCode = currencyCode;
    }

    public Guid User { get; init; }
    public decimal Value { get; init; }
    public string CurrencyCode { get; init; }
}
