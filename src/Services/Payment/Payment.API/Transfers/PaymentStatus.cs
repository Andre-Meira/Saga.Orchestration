using Domain.Core.Extensions;
using Payment.Core.Domain;

namespace Payment.API.Transfers;

public record PaymentStatus
{
    public PaymentStatus(PaymentEventStream payment)
    {
        IdPayment = payment.IdPayment;
        StatusPayment = payment.Status.ToString();
        StepPayment = payment.Step.GetEnumDescription();
    }

    public Guid IdPayment { get; set; }
    public string StatusPayment { get; set; }
    public string StepPayment { get; set; }
}
