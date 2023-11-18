using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Bank;

public sealed class BankWorker : IConsumer<BankCommand>
{
    private readonly ILogger<BankWorker> _logger;
    private readonly HttpClient _httpClient;

    public BankWorker(
        ILogger<BankWorker> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Task Consume(ConsumeContext<BankCommand> context)
    {
        return Task.CompletedTask;
    }
}

public sealed class BankWorkerDefinition : ConsumerDefinition<BankWorker>
{
    public BankWorkerDefinition()
    {
        EndpointName = "queue-bank";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<BankWorker> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(e => e.Interval(3, TimeSpan.FromSeconds(15)));
    }
}
