using Automatonymous;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Machine.Activitys;

public class CardProcessActivity : IActivity<ICardProcessArguments, ICardProcessLog>
{
    private readonly ILogger<CardProcessActivity> _logger;

    public static readonly Uri Endpoint = new Uri("exchange:card-process_execute");

    public CardProcessActivity(ILogger<CardProcessActivity> logger)
    {
        _logger = logger;
    }

    public Task<CompensationResult> Compensate(CompensateContext<ICardProcessLog> context)
    {
        return Task.FromResult(context.Compensated());
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ICardProcessArguments> context)
    {
        _logger.LogInformation("Card Process.");

        await Task.Delay(TimeSpan.FromSeconds(5));

        _logger.LogInformation("Card Processed.");

        return context.Completed(new { context.Arguments.IdPayment, Message = "Sucesso"});
    }
}


public class ICardProcessArguments
{
    public Guid IdPayment { get; set; }

    public Guid Payeer { get; set; }

    public decimal Value { get; set; }
}

public interface ICardProcessLog
{
    public Guid IdPayment { get; set; }

    public string Message { get; set; }
}