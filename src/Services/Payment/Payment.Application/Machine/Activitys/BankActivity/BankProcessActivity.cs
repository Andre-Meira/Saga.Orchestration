using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Payment.Application.Machine.Activitys.BankActivity;

public sealed class BankProcessActivity : IActivity<OrderBankProcessArguments, IBankLog>
{
    public static readonly Uri Endpoint = new Uri("exchange:bank-process_execute");

    private readonly ILogger<BankProcessActivity> _logger;

    private readonly HttpClient _httpClient;

    public BankProcessActivity(ILogger<BankProcessActivity> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<CompensationResult> Compensate(CompensateContext<IBankLog> context)
    {
        _logger.LogInformation("Bank compasated idPayment: {0}", context.Log.IdPayment);

        await Task.Delay(TimeSpan.FromSeconds(10));        

        return context.Compensated();
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderBankProcessArguments> context)
    {
        _logger.LogInformation("Bank process started id payment: {0}", context.Arguments.IdPayment);

        OrderBankProcessArguments arguments = context.Arguments;
        Uri urlApi = new(_httpClient.BaseAddress!.ToString());

        BankPaymentRequest cardPayment = new BankPaymentRequest(arguments.Payeer, arguments.Value);
        string cardPaymentJson = JsonConvert.SerializeObject(cardPayment);

        StringContent httpContent = new StringContent(cardPaymentJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(urlApi, httpContent).ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Bank process completed id payment: {0}", context.Arguments.IdPayment);
            return context.Completed(new { IdPayment = context.CorrelationId });
        }

        string body = await responseMessage.Content.ReadAsStringAsync();
        _logger.LogWarning("Bank process Faulted, id payment:{0} response:{1}, code:{2}",
                context.Arguments.IdPayment, body, responseMessage.StatusCode);

        return context.Faulted();
    }
}

public record OrderBankProcessArguments(Guid IdPayment, Guid Payeer, decimal Value)
    : ProcessPayment(IdPayment, Payeer, Value);

public interface IBankLog { Guid IdPayment { get; set; } }