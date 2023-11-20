using Domain.Contracts.Extensions;
using Domain.Contracts.Payment;
using Domain.Core.Abstractions.Stream;
using MassTransit;
using Payment.Core.Bank;
using Payment.Core.Bank.Events;
using Payment.Core.Domain;

namespace Payment.Core.Orchestration;

public sealed class OrchestrationWoker :
    IConsumer<PaymentCommand>,
    IConsumer<CardFailed>,
    IConsumer<CardCompleted>,
    IConsumer<BankCompleted>,
    IConsumer<BankFailed>
{
    private readonly IProcessEventStream<PaymentEventStream> _processEvent;
    public OrchestrationWoker(IProcessEventStream<PaymentEventStream> processEvent)
        => _processEvent = processEvent;

    public async Task Consume(ConsumeContext<PaymentCommand> context)
    {
        PaymentCommand payment = context.Message;

        PaymentInitialized paymentInitialized = new PaymentInitialized(
            payment.IdPayment, payment.Payeer, payment.Payee, payment.Value);

        await _processEvent.Include(paymentInitialized).ConfigureAwait(false);
        CardCommand cardCommand = new CardCommand(payment.IdPayment, payment.Payeer, payment.Value);

        ISendEndpoint endpoint = await context.GetSendEndpoint(cardCommand.GetExchange());
        await endpoint.Send(cardCommand).ConfigureAwait(false);

        CardProcessInitialized cardProcessInitialized = new CardProcessInitialized(payment.IdPayment);
        await _processEvent.Include(cardProcessInitialized).ConfigureAwait(false);
    }

    async Task IConsumer<CardFailed>.Consume(ConsumeContext<CardFailed> context)
    {
        Guid idPayment = context.Message.IdPayment;
        CardProcessFailed cardProcessFailed = new CardProcessFailed(idPayment, context.Message.Error);

        await _processEvent.Include(cardProcessFailed).ConfigureAwait(false);
    }

    async Task IConsumer<CardCompleted>.Consume(ConsumeContext<CardCompleted> context)
    {
        Guid idPayment = context.Message.IdPayment;

        CardProcessCompleted cardProcessCompleted = new CardProcessCompleted(idPayment);
        await _processEvent.Include(cardProcessCompleted).ConfigureAwait(false);
        
        PaymentEventStream paymentEvent = await _processEvent.Process(idPayment);
        BankCommand bankCommand = new BankCommand(idPayment, paymentEvent.Payeer, paymentEvent.Value);        

        ISendEndpoint endpoint = await context.GetSendEndpoint(bankCommand.GetExchange());
        await endpoint.Send(bankCommand).ConfigureAwait(false);

        var bankProcessInitialized = new BankProcessInitialized(idPayment);
        await _processEvent.Include(bankProcessInitialized).ConfigureAwait(false);
    }

    async Task IConsumer<BankCompleted>.Consume(ConsumeContext<BankCompleted> context)
    {
        Guid idPayment = context.Message.IdPayment;

        BankProcessCompleted cardProcessCompleted = new BankProcessCompleted(idPayment);
        await _processEvent.Include(cardProcessCompleted).ConfigureAwait(false);      
        
        PaymentCompleted paymentCompleted = new PaymentCompleted(idPayment);
        await _processEvent.Include(paymentCompleted).ConfigureAwait(false);
    }

    async Task IConsumer<BankFailed>.Consume(ConsumeContext<BankFailed> context)
    {
        Guid idPayment = context.Message.IdPayment;
        BankProcessFailed cardProcessFailed = new BankProcessFailed(idPayment, context.Message.Error);

        await _processEvent.Include(cardProcessFailed).ConfigureAwait(false);
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
