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

        services.AddTransient<MongoContext>(e => new MongoContext(configuration));
        services.AddScoped<IPaymentEventsRepositore, PaymentEventsRepostiore>();

        BsonClassMap.RegisterClassMap<PaymentInitialized>();
        BsonClassMap.RegisterClassMap<PaymentFailed>();
        BsonClassMap.RegisterClassMap<PaymentCompleted>();
        BsonClassMap.RegisterClassMap<PaymentReversed>();

        return services;
    }
}
