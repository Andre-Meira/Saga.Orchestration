using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Payment.Infrastructure.Models;
using Payment.Core.Domain.Events;
using Microsoft.Extensions.Options;
using Domain.Core.Options;

namespace Payment.Infrastructure.Context;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;
    public IMongoCollection<IEventStreamBD> Eventos
        => _database.GetCollection<IEventStreamBD>("Eventos");

    public MongoContext(IOptions<MongoOptions> options)
    {
        string connection = options.Value.Connection;
        string dataBaseName = options.Value.DatabaseName;

        var client = new MongoClient(connection);
        _database = client.GetDatabase(dataBaseName);       
    }
}

internal sealed class MongoContextConfiguration
{
    public static void RegisterConfig()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

        BsonClassMap.RegisterClassMap<PaymentProcessInitialized>();
        BsonClassMap.RegisterClassMap<PaymentProcessFailed>();
        BsonClassMap.RegisterClassMap<PaymentProcessCompleted>();
        BsonClassMap.RegisterClassMap<PaymentProcessReversed>();

    }
}

