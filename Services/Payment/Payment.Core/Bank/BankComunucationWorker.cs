using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Bank;

internal sealed class BankComunucationWorker : IConsumer<BankComunucation>
{
    private readonly ILogger<BankComunucationWorker> _logger;
    private readonly HttpClient _httpClient;

    public BankComunucationWorker(
        ILogger<BankComunucationWorker> logger, 
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Task Consume(ConsumeContext<BankComunucation> context)
    {
        throw new NotImplementedException();
    }
}

internal sealed class WokerBankDefinition : ConsumerDefinition<BankComunucationWorker>
{
    public WokerBankDefinition()
    {
        EndpointName = "queue-bank";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<BankComunucationWorker> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(e => e.Interval(3, TimeSpan.FromSeconds(15)))        
    }
}
