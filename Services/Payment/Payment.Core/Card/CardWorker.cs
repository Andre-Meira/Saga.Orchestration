using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Core.Bank.Events;

namespace Payment.Core.Bank;

public sealed class CardWorker : IConsumer<CardCommand>
{
    private readonly ILogger<BankWorker> _logger;
    private readonly HttpClient _httpClient;

    public CardWorker(ILogger<BankWorker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task Consume(ConsumeContext<CardCommand> context)
    {
        await context.Publish(new CardRequestCompleted(context.Message.IdPayment));
    }
}

public sealed class CardWokerDefinition : ConsumerDefinition<CardWorker>
{
    public CardWokerDefinition()
    {
        EndpointName = "queue-card";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CardWorker> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(e => e.Interval(3, TimeSpan.FromSeconds(15)));
    }
}
