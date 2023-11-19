using Domain.Core.Extensions;
using Payment.Core.Domain;

namespace Payment.API.Transfers;

public record PaymentStatus
{
    public PaymentStatus(Guid idPayment, Status statusPayment, PaymentStep step)
    {
        IdPayment = idPayment;
        StatusPayment = statusPayment.ToString();
        StepPayment = step.GetEnumDescription();
    }

    public Guid IdPayment { get; set; }
    public string StatusPayment { get; set; }
    public string StepPayment { get; set; }
}
