using Domain.Core.Abstractions.Stream;
using MongoDB.Driver;
using Payment.Core.Domain;
using Payment.Infrastructure.Context;
using Payment.Infrastructure.Models;

namespace Payment.Infrastructure.Repositores;

internal class PaymentEventsRepostiore : IPaymentEventsRepositore
{
    private readonly MongoContext _context;
    public PaymentEventsRepostiore(MongoContext context) => _context = context;

    public IEnumerable<IEventStream> GetEvents(Guid idPayment)
    {
        List<IEventStream> events = 
            _context.Eventos.Find(e => e.Event.IdCorrelation == idPayment).ToList()
            .OrderBy(e => e.Event.DataProcessed).Select(e => e.Event).ToList();

        return events;
    }

    public Task IncressEvent(IEventStream @event) 
        => _context.Eventos.InsertOneAsync(new IEventStreamBD(@event));

}
