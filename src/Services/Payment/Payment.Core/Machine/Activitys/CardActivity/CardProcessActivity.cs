using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Payment.Core.Machine.Activitys.CardActivity;

public class CardProcessActivity : IActivity<OrderCardProcessArguments, ICardProcessLog>
{
    public static readonly Uri Endpoint = new Uri("exchange:card-process_execute");


    private readonly ILogger<CardProcessActivity> _logger;
    private readonly HttpClient _httpClient;

    public CardProcessActivity(ILogger<CardProcessActivity> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Task<CompensationResult> Compensate(CompensateContext<ICardProcessLog> context)
    {
        return Task.FromResult(context.Compensated());
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderCardProcessArguments> context)
    {
        OrderCardProcessArguments arguments = context.Arguments;
        Uri urlApi = new(_httpClient.BaseAddress!.ToString());

        CardPaymentRequest cardPayment = new CardPaymentRequest(arguments.Payeer, arguments.Value);
        string cardPaymentJson = JsonConvert.SerializeObject(cardPayment);

        StringContent httpContent = new StringContent(cardPaymentJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(urlApi, httpContent).ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Card process completed.");
            return context.Completed();
        }

        string body = await responseMessage.Content.ReadAsStringAsync();
        _logger.LogWarning("Card process Faulted response:{0}, code:{1}",
                body, responseMessage.StatusCode);

        return context.Faulted();
    }
}

public record OrderCardProcessArguments(Guid IdPayment, Guid Payeer, decimal Value)
    : OrderPayment(IdPayment, Payeer, Value);

public interface ICardProcessLog
{
    public Guid IdPayment { get; set; }

    public string Message { get; set; }
}