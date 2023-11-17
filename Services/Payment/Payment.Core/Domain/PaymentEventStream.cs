using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;


public sealed class PaymentEventStream : IAggregateStream
{
    private IEventStream? State;

    public Guid Guid => new Guid();

    public Guid IdPayment { get; set; }    
    public Guid Payer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }

    public Status Status { get; private set; }
    public DateTime Date => DateTime.Now;
    public string? StateName => State?.Name;

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
        State = @event;
        Status = Status.Process;
        IdPayment = @event.IdPayment;
        Payer = @event.Payer;
        Payee = @event.Payee;
        Value = @event.Value;
    }

    private void Apply(PaymentCompleted @event)
    {
        State = @event;
        Status = Status.Sucess;
    }

    private void Apply(PaymentFailed @event)
    {
        State = @event;
        Mensagem = @event.Mensagem;
        Status = Status.Fail;
    }

    private void Apply(PaymentReversed @event)
    {
        State = @event;
        Status = Status.Reversal;
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