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

    public IEnumerable<IPaymentEventStream> GetEvents(Guid idPayment)
    {       
        FilterDefinition<IEventStreamBD> filter = Builders<IEventStreamBD>.Filter
            .Eq(x => x.Event.IdCorrelation, idPayment);

        List<IPaymentEventStream> events = _context.Eventos.Find(filter)
            .ToList()
            .OrderBy(e => e.Event.DataProcessed)
            .Select(e => (IPaymentEventStream)e.Event).ToList();

        return events;
    }

    public Task IncressEvent(IPaymentEventStream @event) 
        => _context.Eventos.InsertOneAsync(new IEventStreamBD(@event));

}
