using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Payment.Infrastructure.Models;

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
