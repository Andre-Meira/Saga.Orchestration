using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;

internal record PaymentInitialized(
    Guid IdPayment,
    Guid Payer,
    Guid Payee,
    decimal Value) : IEventStream;

internal record PaymentFailed(Guid IdPayment, string Mensagem) : IEventStream { }

internal record PaymentCompleted(Guid IdPayment) : IEventStream { }

internal record PaymentReversed(Guid IdPayment, string Mensagem) : IEventStream { }
