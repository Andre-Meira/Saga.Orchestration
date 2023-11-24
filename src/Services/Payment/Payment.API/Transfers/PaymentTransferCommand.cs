namespace Payment.API.Transfers;

public record PaymentTransferCommand
{
    public PaymentTransferCommand(Guid payer, Guid payee, decimal value)
    {
        Payer = payer;
        Payee = payee;
        Value = value;
    }

    public Guid Payer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }
}