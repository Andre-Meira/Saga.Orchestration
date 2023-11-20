namespace Payment.Core.Bank.Events;

internal sealed class BankFailed
{
    public BankFailed(
        Guid idPayment, string error)
    {
        IdPayment = idPayment;
        Error = error;
    }

    public Guid IdPayment { get; set; }
    public string Error { get; set; }
}
