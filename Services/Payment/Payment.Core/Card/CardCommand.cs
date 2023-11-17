namespace Payment.Core.Bank;

internal sealed record BankCommand
{
    public BankCommand(
        Guid payeer, 
        decimal value)
    {
        Payeer = payeer;
        Value = value;        
    }

    public Guid Payeer { get; init; }
    public decimal Value { get; init; }    
}
