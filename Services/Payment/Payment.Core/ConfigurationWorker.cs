using Domain.Core.Abstractions.Stream;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payment.Core.Bank;
using Payment.Core.Domain;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net;

namespace Payment.Core;

public static class ConfigurationWorker
{
    public static IServiceCollection AddWorkerService(this IServiceCollection services,
        IConfiguration configuration)
    {
        string urlBank = configuration["url_api_bank"]!;
        string urlCard = configuration["url_api_card"]!;

        services.AddHttpClient<BankWorker>(e =>
        {
            e.BaseAddress = new Uri(urlBank);
            e.DefaultRequestHeaders.Add("Accept", "application/json");
            e.Timeout = TimeSpan.FromMinutes(1);
        })
        .AddPolicyHandler(CreatePolicyError(5, services));


        services.AddHttpClient<CardWorker>(e =>
        {
            e.BaseAddress = new Uri(urlCard);
            e.DefaultRequestHeaders.Add("Accept", "application/json");
            e.Timeout = TimeSpan.FromMinutes(1);
        })
        .AddPolicyHandler(CreatePolicyError(5, services));

        services.AddScoped<IProcessEventStream<PaymentEventStream>, PaymentProcessEvents>();

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
               .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(10));                
    }
}
