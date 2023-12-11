using Domain.Contracts.Payment;
using MassTransit;
using MassTransit.Courier.Contracts;
using Payment.Application.Machine.Activitys.BankActivity;
using Payment.Application.Machine.Activitys.CardActivity;
using Payment.Application.Machine.Events;
using Payment.Core.Domain;
using Payment.Core.Domain.Events;

namespace Payment.Application.Consumers;

public sealed class PaymentWoker :
    IConsumer<OrderPayment>,
    IConsumer<ProcessPayment>
{
    private readonly IPaymentProcessStream _paymentStream;

    public PaymentWoker(IPaymentProcessStream paymentStream)
    {
        _paymentStream = paymentStream;
    }

    public async Task Consume(ConsumeContext<OrderPayment> context)
    {
        OrderPayment payment = context.Message;

        PaymentInitialized paymentInitialized = new PaymentInitialized(
            payment.IdPayment, payment.Payeer, payment.Payee, payment.Value);

        await context.Publish<IPaymentInitialized>(paymentInitialized).ConfigureAwait(false); ;
    }

    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        RoutingSlipBuilder builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(CardProcessActivity), CardProcessActivity.Endpoint, context.Message);
        builder.AddActivity(nameof(BankProcessActivity), BankProcessActivity.Endpoint, context.Message);


        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Faulted,
            RoutingSlipEventContents.Data,
            e => NotifedFaulted(e, context.Message.IdPayment));

        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.ActivityCompleted,
            RoutingSlipEventContents.None,
            nameof(BankProcessActivity),
            e => NotifedCompleted(e, context.Message.IdPayment));

        RoutingSlip routingSlip = builder.Build();
        await context.Execute(routingSlip).ConfigureAwait(false);
    }

    private async Task NotifedFaulted(ISendEndpoint context, Guid idPayment)
    {
        await context.Send<IProcessPaymentFailed>(new
        {
            IdPayment = idPayment,
            Message = "Não foi possivel concluir o pagamento."
        }).ConfigureAwait(false);
    }

    private async Task NotifedCompleted(ISendEndpoint context, Guid idPayment)
    {        
        await context.Send<IProcessPaymentCompleted>(new { IdPayment = idPayment })
            .ConfigureAwait(false);
    }
}


public sealed class OrchestrationWokerDefinition : ConsumerDefinition<PaymentWoker>
{
    public OrchestrationWokerDefinition()
    {
        EndpointName = "queue-payment";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentWoker> consumerConfigurator,
        IRegistrationContext context)
    {

        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}
