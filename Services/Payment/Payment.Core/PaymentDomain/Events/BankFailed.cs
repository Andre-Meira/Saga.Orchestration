using Domain.Core.Abstractions;

namespace Payment.Core.PaymentDomain.Events;

internal record class BankFailed : IEventStream
{
    public Guid Guid => throw new NotImplementedException();
}
