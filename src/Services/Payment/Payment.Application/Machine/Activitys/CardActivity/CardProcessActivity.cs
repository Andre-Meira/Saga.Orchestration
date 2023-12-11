using Domain.Contracts.Payment;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Payment.Application.Machine.Activitys.CardActivity;
    
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

    public async Task<CompensationResult> Compensate(CompensateContext<ICardProcessLog> context)
    {
        _logger.LogInformation("Card compasated idPayment: {0}", context.Log.IdPayment);

        await Task.Delay(TimeSpan.FromSeconds(10));

        return context.Compensated();
    } 

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderCardProcessArguments> context)
    {
        _logger.LogInformation("Card process started id payment: {0}", context.Arguments.IdPayment);

        OrderCardProcessArguments arguments = context.Arguments;
        Uri urlApi = new(_httpClient.BaseAddress!.ToString());

        CardPaymentRequest cardPayment = new CardPaymentRequest(arguments.Payeer, arguments.Value);
        string cardPaymentJson = JsonConvert.SerializeObject(cardPayment);

        StringContent httpContent = new StringContent(cardPaymentJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(urlApi, httpContent).ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Card process completed id payment: {0}", context.Arguments.IdPayment);
            return context.Completed(new { IdPayment = context.CorrelationId});
        }

        string body = await responseMessage.Content.ReadAsStringAsync();
        _logger.LogWarning("Card process Faulted, id payment:{0} response:{1}, code:{2}",
                context.Arguments.IdPayment, body, responseMessage.StatusCode);        

        return context.Faulted();
    }
}

public record OrderCardProcessArguments(Guid IdPayment, Guid Payeer, decimal Value)
    : ProcessPayment(IdPayment, Payeer, Value);

public interface ICardProcessLog
{
    public Guid IdPayment { get; set; }

    public string Message { get; set; }
}