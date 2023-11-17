using Domain.Contracts.Payment;
using Domain.Core.Abstractions.Stream;
using MassTransit;
using Payment.Core.Domain;

namespace Payment.Core.Orchestration;

public sealed class OrchestrationWoker : IConsumer<PaymentCommand>
{
    private readonly IProcessEventStream<PaymentEventStream> _processEvent;
    private readonly IPublishEndpoint _publishEndpoint;    

    public OrchestrationWoker(
        IPublishEndpoint publishEndpoint,
        IProcessEventStream<PaymentEventStream> processEvent)
    {
        _processEvent = processEvent;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PaymentCommand> context)
    {
        PaymentCommand payment = context.Message;

        PaymentInitialized paymentInitialized = new PaymentInitialized(
            payment.IdPayment,payment.Payer, payment.Payee, payment.Value);

        await _processEvent.Process(paymentInitialized).ConfigureAwait(false);  
        await _publishEndpoint.Publish(payment).ConfigureAwait(false);
    }
}

public sealed class OrchestrationWokerDefinition : ConsumerDefinition<OrchestrationWoker>
{
    public OrchestrationWokerDefinition()
    {
        EndpointName = "queue-orchestration";
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<OrchestrationWoker> consumerConfigurator, 
        IRegistrationContext context)
    {        

        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}
