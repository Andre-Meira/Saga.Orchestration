using Domain.Core.Abstractions.Stream;
using System.ComponentModel;

namespace Payment.Core.Domain;

public sealed class PaymentEventStream : IAggregateStream
{
    #region Prop
    public Guid Guid => Guid.NewGuid();

    public Guid IdPayment { get; set; }
    public Guid Payeer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }
    public Status Status { get; private set; }
    public PaymentStep Step { get; private set; }
    public string StatusName { get { return Status.ToString(); } private set { } }

    public bool IsReversal => Status == Status.Complet;

    public bool IsRetry => Status == Status.Fail;

    public DateTime Date { get; private set; }
    public string? Mensagem { get; private set; }
    #endregion

    public void When(IEventStream @event)
    {
        switch (@event)
        {
            case PaymentInitialized payment:
                Apply(payment);
                break;

            case PaymentCompleted paymentCompleted:
                Apply(paymentCompleted);
                break;

            case PaymentReversed paymentReversed:
                Apply(paymentReversed);
                break;

            case CardProcessInitialized cardProcessInitialized:
                Apply(cardProcessInitialized);
                break;

            case CardProcessCompleted cardProcessCompleted:
                Apply(cardProcessCompleted);
                break;

            case CardProcessFailed cardProcessFailed:
                Apply(cardProcessFailed);
                break;

            case BankProcessInitialized bankProcessInitialized:
                Apply(bankProcessInitialized);
                break;

            case BankProcessCompleted bankProcessCompleted:
                Apply(bankProcessCompleted);
                break;

            case BankProcessFailed bankProcessFailed:
                Apply(bankProcessFailed);
                break;
        }
    }

    #region Apply Events
    private void Apply(PaymentInitialized @event)
    {
        Status = @event.Status;
        IdPayment = @event.IdPayment;
        Payeer = @event.Payer;
        Payee = @event.Payee;
        Value = @event.Value;
        Date = DateTime.Now;
        Step = @event.PaymentStep;
    }

    private void Apply(PaymentCompleted @event)
    {
        Status = @event.Status;
        Step = @event.PaymentStep;
    }

    private void Apply(PaymentReversed @event)
    {
        if (IsReversal == false)
            throw new Exception("");

        Status = @event.Status;
        Step = @event.PaymentStep;
        Value = 0;
    }
   
    private void Apply(CardProcessInitialized @event)
    {                
        Step = @event.PaymentStep;
    }
    
    private void Apply(CardProcessCompleted @event)
    {
        Step = @event.PaymentStep;
    }

    private void Apply(CardProcessFailed @event)
    {
        Mensagem = @event.Message;
        Status = Status.Fail;
        Step = @event.PaymentStep;
    }

    private void Apply(BankProcessInitialized @event)
    {
        Step = @event.PaymentStep;
    }

    private void Apply(BankProcessCompleted @event)
    {
        Step = @event.PaymentStep;                
    }

    private void Apply(BankProcessFailed @event)
    {
        Mensagem = @event.Message;
        Status = Status.Fail;
        Step = @event.PaymentStep;
    }
    #endregion
}


public enum PaymentStep {

    [Description("Info Validation")]
    InfoValidation,

    [Description("Card Processing")]
    CardProcessing,

    [Description("Card Complet")]
    CardComplet,

    [Description("Card Fail")]
    CardFail,

    [Description("Bank Processing")]
    BankProcessing,

    [Description("Bank Complet")]
    BankComplet,

    [Description("Bank Fail")]
    BankFail,

    [Description("Complet")]   
    Complet,

    [Description("Reversal")]
    Reversal
}

public enum Status
{
    Process,
    Complet,
    Fail,
    Reversal,
    Retry
}