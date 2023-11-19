namespace Payment.Core.Bank.Events;

internal sealed class CardCompleted
{
    public CardCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
    }

    public Guid IdPayment { get; set; }

    public Guid Code { get; set; }
}
