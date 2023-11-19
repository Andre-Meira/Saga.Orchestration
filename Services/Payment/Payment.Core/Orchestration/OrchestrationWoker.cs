﻿using Domain.Contracts.Extensions;
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
    IConsumer<BankRequestCompleted>,
    IConsumer<BankRequestFailied>
{
    private readonly IProcessEventStream<PaymentEventStream> _processEvent;
    public OrchestrationWoker(IProcessEventStream<PaymentEventStream> processEvent)
        => _processEvent = processEvent;

    public async Task Consume(ConsumeContext<PaymentCommand> context)
    {
        PaymentCommand payment = context.Message;

        PaymentInitialized paymentInitialized = new PaymentInitialized(
            payment.IdPayment, payment.Payer, payment.Payee, payment.Value);

        await _processEvent.Include(paymentInitialized).ConfigureAwait(false);

        CardCommand cardCommand = new CardCommand(payment.IdPayment, payment.Payer, payment.Value);

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
    }

    Task IConsumer<BankRequestCompleted>.Consume(ConsumeContext<BankRequestCompleted> context)
    {
        throw new NotImplementedException();
    }

    Task IConsumer<BankRequestFailied>.Consume(ConsumeContext<BankRequestFailied> context)
    {
        throw new NotImplementedException();
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
