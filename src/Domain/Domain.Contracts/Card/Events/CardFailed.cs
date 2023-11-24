namespace Domain.Contracts.Card.Events;

public sealed class CardFailed
{
    public CardFailed(
        Guid idPayment, string error)
    {
        IdPayment = idPayment;
        Error = error;
    }

    public Guid IdPayment { get; set; }
    public string Error { get; set; }
}
