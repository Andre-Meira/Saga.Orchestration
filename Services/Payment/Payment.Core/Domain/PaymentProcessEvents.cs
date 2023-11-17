using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Logging;
using Payment.Core.Domain;

namespace Payment.Core.Domain;

internal sealed class PaymentProcessEvents : IProcessEventStream<PaymentEventStream>
{
    private readonly ILogger<PaymentProcessEvents> _logger;
    private readonly List<PaymentEventStream> _paymentAggregateStreams;    

    public PaymentProcessEvents(ILogger<PaymentProcessEvents> logger)
    {
        _paymentAggregateStreams = new List<PaymentEventStream>();
        _logger = logger;        
    }
    
    public Task Process(IEventStream @event)
    {
        PaymentEventStream paymentAggregate = new PaymentEventStream();
        paymentAggregate.When(@event);
        
        return Task.CompletedTask;
    }

    public PaymentEventStream? LastEvent()
    {
        return _paymentAggregateStreams.OrderByDescending(e => e.Date).LastOrDefault();
    }

    public IEnumerable<PaymentEventStream> GetEvents(Guid Id)
    {
        throw new NotImplementedException();
    }
}
