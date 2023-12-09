using Domain.Contracts.Notification;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NotificationService;

internal sealed class HandlerNotification : BackgroundService
{
    private readonly ILogger<HandlerNotification> _logger;

    public HandlerNotification(ILogger<HandlerNotification> logger)
    {
        _logger = logger;
    }

    public Task PaymentInitilized(PaymentInitializedNotification notification)
    {
        _logger.LogInformation("Notification recive {0}, event payment initilized, idPayment: {1}, value: {2}", 
            notification.IdNotification, notification.IdPayment, notification.Value);

        return Task.CompletedTask;
    }

    public Task PaymentComplted(PaymentCompletedNotification notification)
    {
        _logger.LogInformation("Notification recive {0}, payment completed, idPayment: {1}",
            notification.IdNotification, notification.IdPayment);

        return Task.CompletedTask;
    }

    public Task PaymentFaulted(PaymentFailedNotification notification)
    {
        _logger.LogError("Notification recive {0}, payment faulted, idPayment: {1}, message: {2}",
            notification.IdNotification, notification.IdPayment, notification.Message);

        return Task.CompletedTask;
    }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5212/payment-notification")
            .Build();

        connection.On<PaymentInitializedNotification>("Initilized", PaymentInitilized);
        connection.On<PaymentCompletedNotification>("Completed", PaymentComplted);
        connection.On<PaymentFailedNotification>("Faulted", PaymentFaulted);

        connection.Closed += async (error) =>
        {
            Console.WriteLine("A conexão foi fechada. Tentando reconectar...");            
            await connection.StartAsync();
        };

        connection.Reconnecting += (error) =>
        {
            Console.WriteLine("Tentando reconectar...");
            return Task.CompletedTask;
        };        

        await connection.StartAsync();        
    }    
}
