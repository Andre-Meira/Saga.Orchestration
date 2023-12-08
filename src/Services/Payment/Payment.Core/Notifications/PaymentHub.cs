using Domain.Contracts.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Payment.Core.Notifications;

public sealed class PaymentHub : Hub<IPaymentNotification>, IPaymentNotification
{    
    private readonly ILogger<PaymentHub> _logger;

    public PaymentHub(ILogger<PaymentHub> logger)
    {
        _logger = logger;
    }

    public Task Completed(IPaymentCompletedNotification notification)
        => Clients.All.Completed(notification);

    public Task Faulted(IPaymentFailedNotification notification)
        => Clients.All.Faulted(notification);
    

    public Task Initilized(IPaymentInitializedNotification notification)
        => Clients.All.Initilized(notification);

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation("{0} connected hub notification payment", connectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation("{0} connection exception {1}", connectionId, exception?.Message);        
        return base.OnDisconnectedAsync(exception); 
    }
}
