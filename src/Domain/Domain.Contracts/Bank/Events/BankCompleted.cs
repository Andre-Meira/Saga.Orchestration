namespace Domain.Contracts.Bank.Events;

public sealed class BankCompleted
{
    public BankCompleted(
        Guid idPayment)
    {
        IdPayment = idPayment;
    }

    public Guid IdPayment { get; set; }
}
