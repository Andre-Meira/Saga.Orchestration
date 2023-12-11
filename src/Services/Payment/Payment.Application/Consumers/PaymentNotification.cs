using Domain.Contracts.Notification;
using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Payment.Application.Notifications;

namespace Payment.Application.Consumers;

public sealed class PaymentNotification :
    IConsumer<IPaymentInitialized>,
    IConsumer<IPaymentCompleted>,
    IConsumer<IPaymentFailed>

{
    private readonly IHubContext<PaymentHub, IPaymentNotification> _paymentNotification;
    private readonly ILogger<PaymentNotification> _logger;

    public PaymentNotification(
        IHubContext<PaymentHub, IPaymentNotification> paymentNotification,  
        ILogger<PaymentNotification> logger)
    {
        _logger = logger;
        _paymentNotification = paymentNotification;        
    }

    public async Task Consume(ConsumeContext<IPaymentInitialized> context)
    {        
        var notification = new PaymentInitializedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}", 
            nameof(PaymentInitializedNotification), context.Message.IdPayment);

        await _paymentNotification.Clients.All.Initilized(notification);
    }

    public async Task Consume(ConsumeContext<IPaymentCompleted> context)
    {
        var notification = new PaymentCompletedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}",
            nameof(PaymentCompletedNotification), context.Message.IdPayment);

        await _paymentNotification.Clients.All.Completed(notification);
    }

    public async Task Consume(ConsumeContext<IPaymentFailed> context)
    {
        var notification = new PaymentFailedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}",
            nameof(PaymentFailedNotification), context.Message.IdPayment);

        await _paymentNotification.Clients.All.Faulted(notification);
    }
}

public sealed class PaymentNotificationWokerDefinition : ConsumerDefinition<PaymentNotification>
{
    public PaymentNotificationWokerDefinition()
    {
        EndpointName = "queue-payment-notification";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentNotification> consumerConfigurator,
        IRegistrationContext context)
    {

        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}
