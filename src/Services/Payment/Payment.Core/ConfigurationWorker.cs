using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain;
using Payment.Core.Machine;
using Payment.Core.Machine.Activitys;

namespace Payment.Core;

public static class ConfigurationWorker
{
    public static IServiceCollection AddWorkerService(
        this IServiceCollection services,
        IConfiguration configuration)
    {        
        services.AddScoped<IPaymentProcessStream, PaymentProcessStream>();
        services.AddScoped<OrderPaymentMachineActivity>();
        //services.AddScoped<CardProcessActivity>();

        return services;
    }

}
