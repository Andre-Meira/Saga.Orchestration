using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Bank;

internal sealed class CardWorker : IConsumer<CardWorker>
{
    private readonly ILogger<BankWorker> _logger;
    private readonly HttpClient _httpClient;

    public CardWorker(
        ILogger<BankWorker> logger, 
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Task Consume(ConsumeContext<CardWorker> context)
    {
        return Task.CompletedTask;
    }
}

internal sealed class WokerCardDefinition : ConsumerDefinition<CardWorker>
{
    public WokerCardDefinition()
    {
        EndpointName = "queue-bank";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CardWorker> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(e => e.Interval(3, TimeSpan.FromSeconds(15)))        
    }
}
