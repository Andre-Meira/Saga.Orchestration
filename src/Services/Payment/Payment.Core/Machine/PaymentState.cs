using MassTransit;

namespace Payment.Core.Machine;

public class PaymentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public Guid Payeer { get; set; }

    public Guid Payee { get; set; }

    public decimal Value { get; set; }

    public DateTime Date { get; set; }

    public string? FaultReason { get; set; }
    public string? CurrentState { get; set; }

}
