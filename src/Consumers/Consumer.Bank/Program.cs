using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;
using Polly;
using System.Net;
using Consumer.Bank;
using Domain.Core.Observability;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);                
        })
         .ConfigureServices((hostContext, services) =>
        {
            services.AddTracing("Consumer.Bank", hostContext.Configuration);

            services.AddMassTransit(bus =>
            {
                bus.AddConsumer<BankWorker>(typeof(BankWorkerDefinition));

                bus.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h => { h.Username("guest"); h.Password("guest"); });
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddHttpClient<BankWorker>(e =>
            {
                e.BaseAddress = new Uri("https://c35b1d17-da50-4adb-a8bc-25e9806181e6.mock.pstmn.io/card-payment");
                e.DefaultRequestHeaders.Add("Accept", "application/json");
                e.Timeout = TimeSpan.FromMinutes(1);
            })
            .AddPolicyHandler(CreatePolicyError(5, services));
        });
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