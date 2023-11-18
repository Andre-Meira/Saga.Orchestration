using Domain.Contracts;
using MassTransit;

namespace Payment.Core.Bank;

[EntityName(nameof(BankCommand))]
public sealed record BankCommand : IContract
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
