using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Machine.Events;
using Payment.Core.Domain;

namespace Payment.Application.Machine.Activitys;

internal class PaymentFaliedMachineActivity :
    IStateMachineActivity<PaymentState, IProcessPaymentFailed>
{
    private readonly ILogger<PaymentFaliedMachineActivity> _logger;
    private readonly IPublishEndpoint _publishEndpoint;    

    public PaymentFaliedMachineActivity(
        IPublishEndpoint publishEndpoint,
        ILogger<PaymentFaliedMachineActivity> logger)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
    public void Probe(ProbeContext context) => context.CreateScope("payment");

    public async Task Execute(BehaviorContext<PaymentState, IProcessPaymentFailed> context,
        IBehavior<PaymentState, IProcessPaymentFailed> next)
    {
        _logger.LogInformation("Payment failed id:{0}", context.Message.IdPayment);

        await _publishEndpoint.Publish<IPaymentFailed>(new 
        { 
            context.Message.IdPayment,
            context.Message.Message,  
        })
        .ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<PaymentState, IProcessPaymentFailed, TException> context,
        IBehavior<PaymentState, IProcessPaymentFailed> next) where TException : Exception
    {
        _logger.LogWarning("Faulted, id {0}, message: {1}",
            context.Saga.CorrelationId, context.Exception.Message);

        return next.Faulted(context);
    }
}
