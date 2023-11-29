using Domain.Contracts.Extensions;
using Domain.Contracts.Payment;
using MassTransit;
using Domain.Contracts.Bank.Events;
using Payment.Core.Domain;
using Domain.Contracts.Card.Events;
using Domain.Contracts.Card;
using Domain.Contracts.Bank;
using Payment.Core.Domain.Events;

namespace Payment.Core.Orchestration;

public sealed class OrchestrationWoker :
    IConsumer<PaymentCommand>,
    IConsumer<CardFailed>,
    IConsumer<CardCompleted>,
    IConsumer<BankCompleted>,
    IConsumer<BankFailed>
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

        await _paymentStream.Include(paymentInitialized).ConfigureAwait(false);
        CardCommand cardCommand = new CardCommand(payment.IdPayment, payment.Payeer, payment.Value);

        ISendEndpoint endpoint = await context.GetSendEndpoint(cardCommand.GetExchange());
        await endpoint.Send(cardCommand).ConfigureAwait(false);

        CardProcessInitialized cardProcessInitialized = new CardProcessInitialized(payment.IdPayment);
        await _paymentStream.Include(cardProcessInitialized).ConfigureAwait(false);
    }

    async Task IConsumer<CardFailed>.Consume(ConsumeContext<CardFailed> context)
    {
        Guid idPayment = context.Message.IdPayment;
        CardProcessFailed cardProcessFailed = new CardProcessFailed(idPayment, context.Message.Error);

        await _paymentStream.Include(cardProcessFailed).ConfigureAwait(false);
    }

    async Task IConsumer<CardCompleted>.Consume(ConsumeContext<CardCompleted> context)
    {
        Guid idPayment = context.Message.IdPayment;

        CardProcessCompleted cardProcessCompleted = new CardProcessCompleted(idPayment);
        await _paymentStream.Include(cardProcessCompleted).ConfigureAwait(false);
        
        PaymentStream paymentEvent = await _paymentStream.Process(idPayment);
        BankCommand bankCommand = new BankCommand(idPayment, paymentEvent.Payeer, paymentEvent.Value);        

        ISendEndpoint endpoint = await context.GetSendEndpoint(bankCommand.GetExchange());
        await endpoint.Send(bankCommand).ConfigureAwait(false);

        var bankProcessInitialized = new BankProcessInitialized(idPayment);
        await _paymentStream.Include(bankProcessInitialized).ConfigureAwait(false);
    }

    async Task IConsumer<BankCompleted>.Consume(ConsumeContext<BankCompleted> context)
    {
        Guid idPayment = context.Message.IdPayment;

        BankProcessCompleted cardProcessCompleted = new BankProcessCompleted(idPayment);
        await _paymentStream.Include(cardProcessCompleted).ConfigureAwait(false);      
        
        PaymentCompleted paymentCompleted = new PaymentCompleted(idPayment);
        await _paymentStream.Include(paymentCompleted).ConfigureAwait(false);
    }

    async Task IConsumer<BankFailed>.Consume(ConsumeContext<BankFailed> context)
    {
        Guid idPayment = context.Message.IdPayment;
        BankProcessFailed cardProcessFailed = new BankProcessFailed(idPayment, context.Message.Error);

        await _paymentStream.Include(cardProcessFailed).ConfigureAwait(false);
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
