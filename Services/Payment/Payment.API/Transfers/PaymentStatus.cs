using Payment.Core.Domain;

namespace Payment.API.Transfers;

public record PaymentStatus
{
    public PaymentStatus(Guid idPayment, Status statusPayment)
    {
        IdPayment = idPayment;
        StatusPayment = statusPayment.ToString();
    }

    public Guid IdPayment { get; set; }
    public string StatusPayment { get; set; }
}
