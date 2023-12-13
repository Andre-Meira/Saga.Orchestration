using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Payment.Core.Domain;
using Payment.Infrastructure.Context;
using Payment.Infrastructure.Repositores;

namespace Payment.Infrastructure;

public static class InfrastructureImplementation
{
    public static IServiceCollection ConfigureInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var objectSerializer = new ObjectSerializer(x => true);
        BsonSerializer.RegisterSerializer(objectSerializer);

        MongoContextConfiguration.RegisterConfig();   
        services.AddTransient<MongoContext>();
        services.AddScoped<IPaymentEventsRepositore, PaymentEventsRepostiore>();


        return services;
    }
}
