using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;


public interface IPaymentEventStream : IEventStream
{
    public void Process(PaymentStream stream);
}
