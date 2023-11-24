namespace Domain.Contracts.Card.Events;

public sealed class CardCompleted
{
    public CardCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
    }

    public Guid IdPayment { get; set; }

    public Guid Code { get; set; }
}
