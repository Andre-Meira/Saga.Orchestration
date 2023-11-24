using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;

public interface IPaymentEventsRepositore
{
    public IEnumerable<IEventStream> GetEvents(Guid idPayment);

    public Task IncressEvent(IEventStream @event);
}
