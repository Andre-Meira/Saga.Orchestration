using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Machine.Events;
using Payment.Core.Domain;
using Payment.Core.Domain.Events;

namespace Payment.Application.Machine.Activitys;

internal sealed class PaymentCompletedMachineActivity :
    IStateMachineActivity<PaymentState, IProcessPaymentCompleted>
{
    private readonly ILogger<PaymentCompletedMachineActivity> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IPaymentProcessStream _paymentProcess;    

    public PaymentCompletedMachineActivity(ILogger<PaymentCompletedMachineActivity> logger, 
        IPublishEndpoint publishEndpoint, IPaymentProcessStream paymentProcess)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _paymentProcess = paymentProcess;
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
    public void Probe(ProbeContext context) => context.CreateScope("payment");

    public async Task Execute(BehaviorContext<PaymentState, IProcessPaymentCompleted> context, 
        IBehavior<PaymentState, IProcessPaymentCompleted> next)
    {
        Guid idPayment = context.Message.IdPayment;

        _logger.LogInformation("Payment completed id:{0}", idPayment);

        await _paymentProcess.Include(new PaymentProcessCompleted(idPayment, "Pagamento realizado."));

        await _publishEndpoint.Publish<IPaymentCompleted>(new {idPayment})
            .ConfigureAwait(false);

        await next.Execute(context);
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
