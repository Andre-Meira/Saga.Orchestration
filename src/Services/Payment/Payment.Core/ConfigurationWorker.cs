using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain;

namespace Payment.Core;

public static class ConfigurationWorker
{
    public static IServiceCollection AddWorkerService(
        this IServiceCollection services,
        IConfiguration configuration)
    {        
        services.AddScoped<IProcessEventStream<PaymentEventStream>, PaymentProcessEvents>();

        return services;
    }

}
