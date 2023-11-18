using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Domain;

internal sealed class PaymentProcessEvents : IProcessEventStream<PaymentEventStream>
{
    private readonly ILogger<PaymentProcessEvents> _logger;
    private readonly IPaymentEventsRepositore _paymentEvents;

    public PaymentProcessEvents(
        ILogger<PaymentProcessEvents> logger,
        IPaymentEventsRepositore paymentEvents)
    {
        _logger = logger;
        _paymentEvents = paymentEvents;
    }

    public IEnumerable<IEventStream> GetEvents(Guid Id)
    {
        return _paymentEvents.GetEvents(Id);
    }

    public async Task Include(IEventStream @event)
    {
        await _paymentEvents.IncressEvent(@event).ConfigureAwait(false);
    }

    public Task<PaymentEventStream> Process(Guid Id)
    {
        IEnumerable<IEventStream> events = GetEvents(Id);
        PaymentEventStream paymentEvent = new PaymentEventStream();

        foreach (IEventStream @event in events) paymentEvent.When(@event);

        return Task.FromResult(paymentEvent);
    }
}
