using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Bank;

internal sealed class BankWorker : IConsumer<BankCommand>
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

internal sealed class WokerBankDefinition : ConsumerDefinition<BankWorker>
{
    public WokerBankDefinition()
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
