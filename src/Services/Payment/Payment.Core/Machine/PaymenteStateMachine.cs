using Domain.Contracts.Payment;
using MassTransit;
using Payment.Core.Domain.Events;

namespace Payment.Core.Machine;

public sealed class PaymenteStateMachine : MassTransitStateMachine<PaymentState>
{
    public PaymenteStateMachine()
    {
        InstanceState(e => e.CurrentState);

        Event(() => PaymentInitialized, x => x.CorrelateById(m => m.Message.IdPayment));

        Initially(
            When(PaymentInitialized)
                .Then(context =>
                {
                    context.Saga.CreationDate = DateTime.Now;
                    context.Saga.Date = DateTime.Now;
                    context.Saga.Payee = context.Message.Payee;
                    context.Saga.Payeer = context.Message.Payeer;
                    context.Saga.Value = context.Message.Value;
                    context.Saga.CorrelationId = context.Message.IdPayment;
                })
                .Activity(e => e.OfType<OrderPaymentMachineActivity>())
                .TransitionTo(Submitted));
        During(Submitted,
            When(PaymentFailed)
                .Then(context =>
                {
                    context.Saga.FaultReason = context.Message.Mensagem;
                    context.Saga.Date = DateTime.Now;
                })
                .TransitionTo(Faulted),

            When(PaymentCompleted)
                .Then(context =>
                {
                    context.Saga.Date = DateTime.Now;
                })
                .TransitionTo(Completed));
    }

    public State? Submitted { get; private set; }
    public State? Completed { get; private set; }
    public State? Canceled { get; private set; }
    public State? Faulted { get; private set; }

    public Event<IPaymentInitialized>? PaymentInitialized { get; private set; }
    public Event<IPaymentFailed>? PaymentFailed { get; private set; }
    public Event<IPaymentCompleted>? PaymentCompleted { get; private set; }
}

