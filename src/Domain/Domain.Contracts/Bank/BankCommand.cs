using MassTransit;

namespace Domain.Contracts.Bank;

[EntityName(nameof(BankCommand))]
public sealed record BankCommand : IContract
{
    public BankCommand(Guid idPayment, Guid payeer, decimal value)
    {
        IdPayment = idPayment;
        Payeer = payeer;
        Value = value;
    }

    public Guid IdPayment { get; init; }
    public Guid Payeer { get; init; }
    public decimal Value { get; init; }
}
