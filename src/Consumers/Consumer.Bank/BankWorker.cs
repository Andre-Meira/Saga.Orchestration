using Domain.Contracts.Bank;
using Domain.Contracts.Bank.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Consumer.Bank;

public sealed class BankWorker : IConsumer<BankCommand>
{
    private readonly ILogger<BankWorker> _logger;
    private readonly HttpClient _httpClient;

    public BankWorker(ILogger<BankWorker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task Consume(ConsumeContext<BankCommand> context)
    {
        BankCommand message = context.Message;
        BankPaymentRequest bankPayment = new BankPaymentRequest(message.Payeer, message.Value);
        string cardPaymentJson = JsonConvert.SerializeObject(bankPayment);

        Uri uriApi = new Uri(_httpClient.BaseAddress!.ToString());
        StringContent httpContent = new StringContent(cardPaymentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await _httpClient
            .PostAsync(uriApi, httpContent)
            .ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            BankCompleted completed = new BankCompleted(message.IdPayment);
            await context.Publish(completed).ConfigureAwait(false); return;
        }

        string body = await responseMessage.Content.ReadAsStringAsync();

        BankFailed failed = new BankFailed(message.IdPayment, body);
        await context.Publish(failed).ConfigureAwait(false); return;
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
