using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payment.Core.Bank.Events;
using Payment.Core.Card;
using System.Text;

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
        CardCommand message = context.Message;
        CardPaymentRequest cardPayment = new CardPaymentRequest(message.Payeer, message.Value);
        string cardPaymentJson = JsonConvert.SerializeObject(cardPayment);

        Uri uriApi = new Uri(_httpClient.BaseAddress!.ToString());
        StringContent httpContent = new StringContent(cardPaymentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await _httpClient
            .PostAsync(uriApi, httpContent)
            .ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode )
        {
            CardCompleted completed = new CardCompleted(message.IdPayment);
            await context.Publish(completed).ConfigureAwait(false); return;
        }

        string body = await responseMessage.Content.ReadAsStringAsync();

        CardFailed failed = new CardFailed(message.IdPayment, body);
        await context.Publish(failed).ConfigureAwait(false); return;        
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
