namespace Payment.Core.Bank.Events;

internal sealed class CardRequestFailied
{
    public CardRequestFailied(
        Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
    }

    public Guid IdPayment { get; set; }
    public string Mensagem { get; set; }
}
