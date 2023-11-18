using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Payment.Core.Domain;

namespace Payment.Infrastructure.Context;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;
    public IMongoCollection<IEventStream> Eventos
        => _database.GetCollection<IEventStream>("Eventos");

    public MongoContext(IConfiguration configuration)
    {
        string connection = configuration["stringMongo"]!;
        string dataBaseName = configuration["dataBaseName"]!;

        var client = new MongoClient(connection);
        _database = client.GetDatabase(dataBaseName);

        BsonClassMap.RegisterClassMap<PaymentInitialized>();
        BsonClassMap.RegisterClassMap<PaymentFailed>();
        BsonClassMap.RegisterClassMap<PaymentCompleted>();
        BsonClassMap.RegisterClassMap<PaymentReversed>();
    }
}
