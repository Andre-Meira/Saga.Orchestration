using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Payment.Infrastructure.Models;
using Payment.Core.Domain;

namespace Payment.Infrastructure.Context;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;
    public IMongoCollection<IEventStreamBD> Eventos
        => _database.GetCollection<IEventStreamBD>("Eventos");

    public MongoContext(IConfiguration configuration)
    {
        string connection = configuration["stringMongo"]!;
        string dataBaseName = configuration["dataBaseName"]!;

        var client = new MongoClient(connection);
        _database = client.GetDatabase(dataBaseName);       
    }
}

internal sealed class MongoContextConfiguration
{
    public static void RegisterConfig()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

        BsonClassMap.RegisterClassMap<PaymentInitialized>();
        BsonClassMap.RegisterClassMap<PaymentCompleted>();
        BsonClassMap.RegisterClassMap<PaymentReversed>();

        BsonClassMap.RegisterClassMap<CardProcessInitialized>();
        BsonClassMap.RegisterClassMap<CardProcessCompleted>();
        BsonClassMap.RegisterClassMap<CardProcessFailed>();

        BsonClassMap.RegisterClassMap<BankProcessInitialized>();
        BsonClassMap.RegisterClassMap<BankProcessCompleted>();
        BsonClassMap.RegisterClassMap<BankProcessFailed>();
    }
}

