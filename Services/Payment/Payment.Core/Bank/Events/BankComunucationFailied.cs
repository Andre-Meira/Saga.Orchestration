namespace Payment.Core.Bank.Events;

internal sealed class BankComunucationFailied
{
    public BankComunucationFailied(
        Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
    }

    public Guid IdPayment { get; set; }
    public string Mensagem { get; set; }
}
