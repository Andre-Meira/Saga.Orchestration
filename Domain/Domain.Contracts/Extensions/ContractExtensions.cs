namespace Domain.Contracts.Extensions;

public static class ContractExtensions
{
    public static Uri GetQueue(this IContract contract) => new Uri($"queue:{nameof(contract)}");

    public static Uri GetExchange(this IContract contract) => new Uri($"exchange:{contract.GetType().Name}");
}
