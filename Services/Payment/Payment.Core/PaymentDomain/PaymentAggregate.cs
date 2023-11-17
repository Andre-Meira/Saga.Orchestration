using Domain.Core.Abstractions;

namespace Payment.Core.Domain;


internal sealed class PaymentAggregate : IAggregateStream
{
    public Guid Guid => new Guid();


    public void When(IEventStream @event)
    {
        throw new NotImplementedException();
    }
}


public enum Status
{
    Process,
    Sucess,
    Fail
}