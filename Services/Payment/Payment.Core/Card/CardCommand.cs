﻿using Domain.Contracts;
using MassTransit;

namespace Payment.Core.Bank;

[EntityName(nameof(CardCommand))]
public sealed record CardCommand : IContract
{
    public CardCommand(
        Guid idPayment,
        Guid payeer,
        decimal value)
    {
        Payeer = payeer;
        Value = value;
        IdPayment = idPayment;
    }

    public Guid IdPayment { get; init; }
    public Guid Payeer { get; init; }
    public decimal Value { get; init; }
}
