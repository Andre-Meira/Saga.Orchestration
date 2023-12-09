using Domain.Contracts.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Notifications;

public sealed class PaymentHub : Hub<IPaymentNotification>
{    
    private readonly ILogger<PaymentHub> _logger;

    public PaymentHub(ILogger<PaymentHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation("{0} connected hub notification payment", connectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogError("{0} connection exception {1}", connectionId, exception?.Message);        
        return base.OnDisconnectedAsync(exception); 
    }
}
