using Domain.Contracts.Payment;
using MassTransit;
using MassTransit.Courier.Contracts;
using Payment.Core.Domain;
using Payment.Core.Domain.Events;
using Payment.Core.Machine.Activitys.BankActivity;
using Payment.Core.Machine.Activitys.CardActivity;

namespace Payment.Core.Consumers;

public sealed class PaymentWoker :
    IConsumer<PaymentCommand>,
    IConsumer<OrderPayment>
{
    private readonly IPaymentProcessStream _paymentStream;

    public PaymentWoker(IPaymentProcessStream paymentStream)
    {
        _paymentStream = paymentStream;
    }

    public async Task Consume(ConsumeContext<PaymentCommand> context)
    {
        PaymentCommand payment = context.Message;

        PaymentInitialized paymentInitialized = new PaymentInitialized(
            payment.IdPayment, payment.Payeer, payment.Payee, payment.Value);

        await context.Publish<IPaymentInitialized>(paymentInitialized).ConfigureAwait(false); ;
    }

    public async Task Consume(ConsumeContext<OrderPayment> context)
    {
        RoutingSlipBuilder builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(CardProcessActivity), CardProcessActivity.Endpoint, context.Message);
        builder.AddActivity(nameof(BankProcessActivity), BankProcessActivity.Endpoint, context.Message);


        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Faulted,
            RoutingSlipEventContents.Data,
            e => NotifedFaulted(context, context.Message.IdPayment));

        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Completed,
            RoutingSlipEventContents.None,
            nameof(BankProcessActivity),
            e => NotifedCompleted(context, context.Message.IdPayment));

        RoutingSlip routingSlip = builder.Build();
        await context.Execute(routingSlip).ConfigureAwait(false);
    }

    private async Task NotifedFaulted(ConsumeContext context, Guid idPayment)
    {
        if (idPayment.Equals(Guid.Empty)) return;

        await context.Publish<IPaymentFailed>(new
        {
            IdPayment = idPayment,
            Message = "Não foi possivel concluir o pagamento."
        }).ConfigureAwait(false);
    }

    private async Task NotifedCompleted(ConsumeContext context, Guid idPayment)
    {
        if (idPayment.Equals(Guid.Empty)) return;

        await context.Publish<IPaymentCompleted>(new { IdPayment = idPayment })
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
