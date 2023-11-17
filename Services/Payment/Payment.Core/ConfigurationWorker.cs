using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Bank;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net;

namespace Payment.Core;

public static class ConfigurationWorker
{
    public static IServiceCollection AddWorkerService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string urlBank = configuration["url_api_bank"]!;

        services.AddHttpClient<BankWorker>(e =>
        {
            e.BaseAddress = new Uri(urlBank);
            e.DefaultRequestHeaders.Add("Accept", "application/json");
            e.Timeout = TimeSpan.FromMinutes(1);
        })
        .AddPolicyHandler(CreatePolicy(5));

        return services;
    }
    public static AsyncRetryPolicy<HttpResponseMessage> CreatePolicy(int retryCount)
    {
        return Policy.HandleResult<HttpResponseMessage>(e => 
                    e.StatusCode == HttpStatusCode.NotFound
                    || e.StatusCode == HttpStatusCode.InternalServerError)
                .RetryAsync(retryCount, onRetry: (message, retryCount) =>
                {
                    string msg = $"Retentativa: {retryCount}";
                    Console.Out.WriteLineAsync(msg);
                }); ;
    }
}
