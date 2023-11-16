namespace Payment.Core.Bank.Events;

internal sealed class BankComunucationCompleted
{
    public BankComunucationCompleted(
        Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
    }

    public Guid IdPayment { get; set; }

    public Guid Code { get; set; }
    public string Mensagem { get; set; }
}
