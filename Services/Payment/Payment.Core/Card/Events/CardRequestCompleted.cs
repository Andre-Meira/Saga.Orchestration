namespace Payment.Core.Bank.Events;

internal sealed class CardRequestCompleted
{
    public CardRequestCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
    }

    public Guid IdPayment { get; set; }

    public Guid Code { get; set; }
}
