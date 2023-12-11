using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Domain;

public interface IPaymentProcessStream
: IProcessorEventStream<PaymentStream, IPaymentEventStream>
{ }

public sealed class PaymentProcessStream : IPaymentProcessStream
{
    private readonly ILogger<PaymentProcessStream> _logger;
    private readonly IPaymentEventsRepositore _paymentEvents;

    public PaymentProcessStream(ILogger<PaymentProcessStream> logger,
        IPaymentEventsRepositore paymentEvents)
    {
        _logger = logger;
        _paymentEvents = paymentEvents;
    }

    public IEnumerable<IPaymentEventStream> GetEvents(Guid Id)
    {
        return _paymentEvents.GetEvents(Id);
    }

    public async Task Include(IPaymentEventStream @event)
    {
        await _paymentEvents.IncressEvent(@event).ConfigureAwait(false);
    }

    public Task<PaymentStream> Process(Guid Id)
    {
        IEnumerable<IPaymentEventStream> events = GetEvents(Id);
        PaymentStream paymentEvent = new PaymentStream();

        foreach (IPaymentEventStream @event in events) paymentEvent.When(@event);

        return Task.FromResult(paymentEvent);
    }
}
