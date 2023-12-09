using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payment.Core.Domain;
using Payment.Core.Machine.Activitys;
using Payment.Core.Machine.Activitys.CardActivity;
using Polly;
using Polly.Extensions.Http;

namespace Payment.Core;

public static class ConfigurationWorker
{
    public static IServiceCollection AddWorkerService(
        this IServiceCollection services,
        IConfiguration configuration)
    {        
        services.AddScoped<IPaymentProcessStream, PaymentProcessStream>();
        services.AddScoped<OrderPaymentMachineActivity>();
        services.AddScoped<PaymentFaliedMachineActivity>();
        services.AddScoped<PaymentCompletedMachineActivity>();
        //services.AddSingleton<IPaymentNotification, PaymentHub>();

        string urlCard = configuration["url_api_bank"]!;

        services.AddHttpClient<CardProcessActivity>(e =>
            {
                e.BaseAddress = new Uri(urlCard);
                e.DefaultRequestHeaders.Add("Accept", "application/json");
                e.Timeout = TimeSpan.FromMinutes(1);
            })
            .AddPolicyHandler(CreatePolicyError(5, services));      

        return services;
    }

    public static IAsyncPolicy<HttpResponseMessage> CreatePolicyError(
        int retryCount,
        IServiceCollection services)
    {
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ILoggerFactory loggerFactory = serviceProvider.GetService<ILoggerFactory>()!;

        return HttpPolicyExtensions
               .HandleTransientHttpError()
               .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
               .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
               .OrResult(msg => msg.StatusCode == HttpStatusCode.ServiceUnavailable)
               .OrResult(msg => msg.StatusCode == HttpStatusCode.RequestTimeout)
               .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(10));
    }

}
