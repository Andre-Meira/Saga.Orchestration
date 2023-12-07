﻿using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Machine.Activitys.BankActivity;

public sealed class BankProcessActivity : IActivity<BankArguments, IBankLog>
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

    public async Task<ExecutionResult> Execute(ExecuteContext<BankArguments> context)
    {        
        await Task.Delay(TimeSpan.FromSeconds(10));   

        _logger.LogInformation("Bank process completed.");
        return context.Faulted(new Exception("Falho aqui.")); //context.Completed(new { IdPayment = context.CorrelationId });
    }
}

public record BankArguments(Guid IdPayment, Guid Payeer, decimal Value)
    : OrderPayment(IdPayment, Payeer, Value);

public interface IBankLog { Guid IdPayment { get; set; } }