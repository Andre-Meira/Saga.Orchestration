namespace Domain.Contracts.Payment;

public interface IPayment
{
    Guid IdPayment { get; init; }
    Guid User { get; init; }  
    decimal Value { get; init; }  
    string CurrencyCode { get; init; }
}
