using Domain.Core.Abstractions.Stream;
using System.ComponentModel;

namespace Payment.Core.Domain;

public sealed class PaymentStream : 
    IAggregateStream<IPaymentEventStream>
{   
    
    public Guid IdPayment { get; set; }
    public Guid Payeer { get; set; }
    public Guid Payee { get; set; }
    public decimal Value { get; set; }
    public Status Status { get; set; }      

    public DateTime Date { get; set; }
    public string? Message { get; set; }    

    public string StatusName { get { return Status.ToString(); } private set { } }

    public bool IsReversal => Status == Status.Complet;

    public bool IsRetry => Status == Status.Fail;

    public void When(IPaymentEventStream @event) => @event.Process(this);

}

public enum Status
{
    Process,
    Complet,
    Fail,
    Reversal
}