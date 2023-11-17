namespace Payment.Core.Bank.Events;

internal sealed class CardRequestCompleted
{
    public CardRequestCompleted(
        Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
    }

    public Guid IdPayment { get; set; }

    public Guid Code { get; set; }
    public string Mensagem { get; set; }
}
