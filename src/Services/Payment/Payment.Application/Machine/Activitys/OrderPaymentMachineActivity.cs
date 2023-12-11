using Domain.Contracts.Extensions;
using Domain.Contracts.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Application.Machine.Activitys;

internal sealed class OrderPaymentMachineActivity : IStateMachineActivity<PaymentState, IPaymentInitialized>
{
    private readonly ILogger<OrderPaymentMachineActivity> _logger;

    public OrderPaymentMachineActivity(ILogger<OrderPaymentMachineActivity> logger)
    {
        _logger = logger;
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public void Probe(ProbeContext context) => context.CreateScope("order-payment");


    public async Task Execute(BehaviorContext<PaymentState, IPaymentInitialized> context,
        IBehavior<PaymentState, IPaymentInitialized> next)
    {
        _logger.LogInformation("Process order {0}", context.Saga.CorrelationId);

        ProcessPayment order = new ProcessPayment(context.Saga.CorrelationId,
            context.Saga.Payeer, context.Saga.Value);

        ConsumeContext consumeContext = context.GetPayload<ConsumeContext>();

        ISendEndpoint sendEndpoint = await consumeContext.GetSendEndpoint(order.GetExchange());
        await sendEndpoint.Send(order).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<PaymentState, IPaymentInitialized, TException> context,
        IBehavior<PaymentState, IPaymentInitialized> next) where TException : Exception
    {
        _logger.LogWarning("Ordem de pagamento falhou, id {0}, message: {1}",
            context.Saga.CorrelationId, context.Exception.Message);

        return next.Faulted(context);
    }

}
