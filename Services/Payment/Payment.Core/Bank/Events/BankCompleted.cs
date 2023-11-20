namespace Payment.Core.Bank.Events;

internal sealed class BankCompleted
{
    public BankCompleted(
        Guid idPayment)
    {
        IdPayment = idPayment;        
    }

    public Guid IdPayment { get; set; }
}
