using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Machine.Events;

namespace Payment.Application.Machine.Activitys;

internal sealed class PaymentCompletedMachineActivity :
    IStateMachineActivity<PaymentState, IProcessPaymentCompleted>
{
    private readonly ILogger<PaymentCompletedMachineActivity> _logger;  
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentCompletedMachineActivity(
        IPublishEndpoint publishEndpoint, 
        ILogger<PaymentCompletedMachineActivity> logger)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
    public void Probe(ProbeContext context) => context.CreateScope("payment");

    public async Task Execute(BehaviorContext<PaymentState, IProcessPaymentCompleted> context, 
        IBehavior<PaymentState, IProcessPaymentCompleted> next)
    {
        _logger.LogInformation("Payment completed id:{0}", context.Message.IdPayment);

        await _publishEndpoint.Publish<IPaymentCompleted>(new {context.Message.IdPayment})
            .ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<PaymentState, IProcessPaymentCompleted, TException> context, 
        IBehavior<PaymentState, IProcessPaymentCompleted> next) where TException : Exception
    {
        _logger.LogWarning("Faulted, id {0}, message: {1}",
            context.Saga.CorrelationId, context.Exception.Message);

        return next.Faulted(context);
    }
}
