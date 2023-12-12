using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Machine.Events;
using Payment.Core.Domain;
using Payment.Core.Domain.Events;

namespace Payment.Application.Machine.Activitys;

internal class PaymentFaliedMachineActivity :
    IStateMachineActivity<PaymentState, IProcessPaymentFailed>
{
    private readonly ILogger<PaymentFaliedMachineActivity> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IPaymentProcessStream _paymentProcess;

    public PaymentFaliedMachineActivity(ILogger<PaymentFaliedMachineActivity> logger, 
        IPublishEndpoint publishEndpoint, IPaymentProcessStream paymentProcess)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _paymentProcess = paymentProcess;
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
    public void Probe(ProbeContext context) => context.CreateScope("payment");

    public async Task Execute(BehaviorContext<PaymentState, IProcessPaymentFailed> context,
        IBehavior<PaymentState, IProcessPaymentFailed> next)
    {
        Guid idPayment = context.Message.IdPayment;
        string message = context.Message.Message;

        _logger.LogInformation("Payment failed id:{0}", idPayment);

        await _paymentProcess.Include(new PaymentProcessFailed(idPayment, message));
        
        await _publishEndpoint.Publish<IPaymentFailed>(new { idPayment,message })
            .ConfigureAwait(false);

        await next.Execute(context);
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
