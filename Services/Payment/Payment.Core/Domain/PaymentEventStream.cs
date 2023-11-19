using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;

public sealed class PaymentEventStream : IAggregateStream
{
    public Guid Guid => Guid.NewGuid();

    public Guid IdPayment { get; set; }
    public Guid Payer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }
    public Status Status { get; private set; }
    public string StatusName { get { return Status.ToString(); } private set { } } 
    public DateTime Date { get; private set;}
    public string? Mensagem;

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

            case PaymentFailed paymentFailed:
                Apply(paymentFailed);
                break;

            case PaymentReversed paymentReversed:
                Apply(paymentReversed);
                break;
        }
    }

    private void Apply(PaymentInitialized @event)
    {
        Status = @event.Status;
        IdPayment = @event.IdPayment;
        Payer = @event.Payer;
        Payee = @event.Payee;
        Value = @event.Value;
        Date = DateTime.Now;
    }

    private void Apply(PaymentCompleted @event)
    {
        Status = @event.Status;
    }

    private void Apply(PaymentFailed @event)
    {
        Mensagem = @event.Mensagem;
        Status = @event.Status;
    }

    private void Apply(PaymentReversed @event)
    {
        Status = @event.Status;
        Value = 0;
    }
}


public enum Status
{
    Process,
    Sucess,
    Fail,
    Reversal,
    Retry
}