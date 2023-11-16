namespace Payment.Core.Bank;

internal sealed record BankComunucation
{
    public BankComunucation(
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
