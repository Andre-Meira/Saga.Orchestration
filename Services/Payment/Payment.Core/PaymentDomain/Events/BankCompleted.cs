using Domain.Core.Abstractions;

namespace Payment.Core.PaymentDomain.Events;

internal class BankCompleted : IEventStream
{
    public Guid Guid => throw new NotImplementedException();
}
