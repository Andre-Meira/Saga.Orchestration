using Domain.Contracts.Payment;
using MassTransit;
using MassTransit.Courier.Contracts;
using Payment.Core.Domain;
using Payment.Core.Domain.Events;
using Payment.Core.Machine.Activitys;

namespace Payment.Core.Orchestration;

public sealed class OrchestrationWoker :
    IConsumer<PaymentCommand>,
    IConsumer<OrderPayment>
{
    private readonly IPaymentProcessStream _paymentStream;

    public OrchestrationWoker(IPaymentProcessStream paymentStream)
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
        ICardProcessArguments cardProcess = new ICardProcessArguments()
        {
            IdPayment = context.Message.IdPayment,
            Payeer = context.Message.Payeer,
            Value = context.Message.Value       
        };

        builder.AddActivity(nameof(CardProcessActivity), CardProcessActivity.Endpoint, cardProcess);

        RoutingSlip routingSlip = builder.Build();

        await context.Execute(routingSlip).ConfigureAwait(false);
    }  
}

public sealed class OrchestrationWokerDefinition : ConsumerDefinition<OrchestrationWoker>
{
    public OrchestrationWokerDefinition()
    {
        EndpointName = "queue-orchestration";
    }

    //protected override void ConfigureConsumer(
    //    IReceiveEndpointConfigurator endpointConfigurator,
    //    IConsumerConfigurator<OrchestrationWoker> consumerConfigurator,
    //    IRegistrationContext context)
    //{

    //    base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    //}
}
