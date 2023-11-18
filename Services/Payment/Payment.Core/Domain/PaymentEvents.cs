using Domain.Core.Abstractions.Stream;

namespace Payment.Core.Domain;

public record PaymentInitialized : IEventStream
{
    public PaymentInitialized(
        Guid idPayment,
        Guid payer,
        Guid payee,
        decimal value)
    {
        IdPayment = idPayment;
        Payer = payer;
        Payee = payee;
        Value = value;
        Status = Status.Process;

        EventName = nameof(PaymentInitialized);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }


    public Guid IdPayment { get; init; }

    public Guid Payer { get; init; }

    public Guid Payee { get; init; }

    public decimal Value { get; init; }

    public Status Status { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record PaymentFailed : IEventStream
{
    public PaymentFailed(Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
        Status = Status.Fail;

        EventName = nameof(PaymentFailed);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }
    public string Mensagem { get; init; }
    public Status Status { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }

}

public record PaymentCompleted : IEventStream
{
    public PaymentCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
        Status = Status.Sucess;

        EventName = nameof(PaymentCompleted);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }
    public Status Status { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record PaymentReversed : IEventStream
{
    public PaymentReversed(Guid idPayment, string mensagem)
    {
        IdPayment = idPayment;
        Mensagem = mensagem;
        Status = Status.Reversal;


        EventName = nameof(PaymentReversed);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }
    public string Mensagem { get; init; }
    public Status Status { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}
