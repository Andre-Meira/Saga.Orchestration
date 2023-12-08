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

    
    //TODO ENVIAR NOTIFICACAO
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = new HubConnectionBuilder()
        .WithUrl("http://localhost:5212/payment-notification")
        .Build();        

        await connection.StartAsync();       
    }
}
