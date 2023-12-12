using Domain.Contracts.Payment;
using MassTransit;
using Payment.Application.Machine.Activitys;
using Payment.Application.Machine.Events;

namespace Payment.Application.Machine;

public sealed class PaymenteStateMachine : MassTransitStateMachine<PaymentState>
{
    public PaymenteStateMachine()
    {
        InstanceState(e => e.CurrentState);

        Event(() => PaymentInitialized, x => x.CorrelateById(m => m.Message.IdPayment));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.IdPayment));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.IdPayment));

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
                    context.Saga.FaultReason = context.Message.Message;
                    context.Saga.Date = DateTime.Now;
                })                
                .Activity(e => e.OfType<PaymentFaliedMachineActivity>())
                .TransitionTo(Faulted),

            When(PaymentCompleted)
                .Then(context => context.Saga.Date = DateTime.Now)                
                .Activity(e => e.OfType<PaymentCompletedMachineActivity>())
                .TransitionTo(Completed));                
    }

    public State? Submitted { get; private set; }
    public State? Completed { get; private set; }
    public State? Canceled { get; private set; }
    public State? Faulted { get; private set; }

    public Event<IPaymentInitialized>? PaymentInitialized { get; private set; }
    public Event<IProcessPaymentFailed>? PaymentFailed { get; private set; }
    public Event<IProcessPaymentCompleted>? PaymentCompleted { get; private set; }
}

