using Domain.Core.Extensions;
using Payment.Core.Domain;

namespace Payment.API.Transfers;

public record PaymentStatus
{
    public PaymentStatus(PaymentStream payment)
    {
        IdPayment = payment.IdPayment;
        Status = payment.Status.ToString();        
        Message = payment.Message;
    }

    public Guid IdPayment { get; set; }
    public string Status { get; set; }    

    public string? Message { get; set; }
}
