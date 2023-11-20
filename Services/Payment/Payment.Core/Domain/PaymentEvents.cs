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
        PaymentStep = PaymentStep.InfoValidation;
    }

    public Guid IdPayment { get; init; }
    public Guid Payer { get; init; }
    public Guid Payee { get; init; }
    public decimal Value { get; init; }
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }    
}

public record PaymentCompleted : IEventStream
{
    public PaymentCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
        Status = Status.Complet;

        EventName = nameof(PaymentCompleted);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
        PaymentStep = PaymentStep.Complet;
    }

    public Guid IdPayment { get; init; }
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }


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
        PaymentStep = PaymentStep.Reversal;

        EventName = nameof(PaymentReversed);
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;        
    }

    public Guid IdPayment { get; init; }
    public string Mensagem { get; init; }
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record CardProcessInitialized : IEventStream
{
    public CardProcessInitialized(Guid idPayment)
    {
        IdPayment = idPayment;        
        PaymentStep = PaymentStep.CardProcessing;
        DataProcessed = DateTime.Now;
        EventName = nameof(CardProcessInitialized);
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }        
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record CardProcessCompleted : IEventStream
{
    public CardProcessCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
        PaymentStep = PaymentStep.CardComplet;
        DataProcessed = DateTime.Now;
        EventName = nameof(CardProcessCompleted);
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record CardProcessFailed : IEventStream
{
    public CardProcessFailed(Guid idPayment, string message)
    {
        IdPayment = idPayment;
        PaymentStep = PaymentStep.CardFail;
        DataProcessed = DateTime.Now;
        EventName = nameof(CardProcessFailed);
        IdCorrelation = idPayment;
        Message = message;
    }

    public Guid IdPayment { get; init; }
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }

    public string Message { get; init; }
}

public record BankProcessInitialized : IEventStream
{
    public BankProcessInitialized(Guid idPayment)
    {
        IdPayment = idPayment;
        PaymentStep = PaymentStep.BankProcessing;
        DataProcessed = DateTime.Now;
        EventName = nameof(BankProcessInitialized);
        IdCorrelation = idPayment;        
    }

    public Guid IdPayment { get; init; }
    public PaymentStep PaymentStep { get; init; }    

    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record BankProcessCompleted : IEventStream
{
    public BankProcessCompleted(Guid idPayment)
    {
        IdPayment = idPayment;
        PaymentStep = PaymentStep.BankComplet;
        DataProcessed = DateTime.Now;
        EventName = nameof(BankProcessCompleted);
        IdCorrelation = idPayment;
    }

    public Guid IdPayment { get; init; }
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}

public record BankProcessFailed : IEventStream
{
    public BankProcessFailed(Guid idPayment, string message)
    {
        IdPayment = idPayment;
        PaymentStep = PaymentStep.BankFail;
        DataProcessed = DateTime.Now;
        EventName = nameof(BankProcessFailed);
        IdCorrelation = idPayment;
        Message = message;
    }

    public Guid IdPayment { get; init; }
    public PaymentStep PaymentStep { get; init; }
    public string Message { get; init; }    


    public DateTime DataProcessed { get; init; }
    public string EventName { get; init; }
    public Guid IdCorrelation { get; init; }
}