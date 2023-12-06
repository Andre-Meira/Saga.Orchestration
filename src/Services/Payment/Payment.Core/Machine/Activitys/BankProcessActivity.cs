using Domain.Contracts.Payment;
using MassTransit;

namespace Payment.Core.Machine.Activitys;

internal sealed class BankProcessActivity :
    IActivity<BankArguments, IBankLog>
{
    public Task<CompensationResult> Compensate(CompensateContext<IBankLog> context)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionResult> Execute(ExecuteContext<BankArguments> context)
    {
        throw new NotImplementedException();
    }
}

public record BankArguments(Guid IdPayment, Guid Payeer, decimal Value)
    : OrderPayment(IdPayment, Payeer, Value);

public interface IBankLog
{

}