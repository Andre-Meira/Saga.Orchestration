using Domain.Core.Abstractions.Stream;
using MongoDB.Driver;
using Payment.Core.Domain;
using Payment.Infrastructure.Context;

namespace Payment.Infrastructure.Repositores;

internal class PaymentEventsRepostiore : IPaymentEventsRepositore
{
    private readonly MongoContext _context;
    public PaymentEventsRepostiore(MongoContext context) => _context = context;

    public IEnumerable<IEventStream> GetEvents(Guid idPayment)
        => _context.Eventos.Find(e => e.IdCorrelation == idPayment).ToList()
        .OrderBy(e => e.DataProcessed);

    public Task IncressEvent(IEventStream @event) => _context.Eventos.InsertOneAsync(@event);

}
