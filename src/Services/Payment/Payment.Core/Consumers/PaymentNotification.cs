using Domain.Contracts.Notification;
using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Core.Notifications;

namespace Payment.Core.Consumers;

public sealed class PaymentNotification :
    IConsumer<IPaymentInitialized>,
    IConsumer<IPaymentCompleted>,
    IConsumer<IPaymentFailed>

{
    private readonly IPaymentNotification _paymentNotification;
    private readonly ILogger<PaymentNotification> _logger;

    public PaymentNotification(
        IPaymentNotification paymentNotification, 
        ILogger<PaymentNotification> logger)
    {
        _logger = logger;
        _paymentNotification = paymentNotification;        
    }

    public async Task Consume(ConsumeContext<IPaymentInitialized> context)
    {        
        IPaymentInitializedNotification notification = 
            new PaymentInitializedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}", 
            nameof(notification), context.Message.IdPayment);

        await _paymentNotification.Initilized(notification);
    }

    public async Task Consume(ConsumeContext<IPaymentCompleted> context)
    {
        IPaymentCompletedNotification notification =
            new PaymentCompletedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}",
            nameof(notification), context.Message.IdPayment);

        await _paymentNotification.Completed(notification);
    }

    public async Task Consume(ConsumeContext<IPaymentFailed> context)
    {
        IPaymentFailedNotification notification =
            new PaymentFailedNotification(context.Message);

        _logger.LogInformation("send notification: {0} payment: {1}",
            nameof(notification), context.Message.IdPayment);

        await _paymentNotification.Faulted(notification);
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
