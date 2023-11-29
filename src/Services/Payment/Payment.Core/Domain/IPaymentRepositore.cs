using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;

public interface IPaymentEventsRepositore
{
    public IEnumerable<IPaymentEventStream> GetEvents(Guid idPayment);

    public Task IncressEvent(IPaymentEventStream @event);
}
