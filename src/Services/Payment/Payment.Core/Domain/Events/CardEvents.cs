namespace Payment.Core.Domain.Events;

public record CardProcessInitialized : IPaymentEventStream
{
    public CardProcessInitialized(Guid idPayment)
    {        
        PaymentStep = PaymentStep.CardProcessing;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
    }
    
    public PaymentStep PaymentStep { get; init; }
    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream) => stream.Step = PaymentStep;

}

public record CardProcessCompleted : IPaymentEventStream
{
    public CardProcessCompleted(Guid idPayment)
    {        
        PaymentStep = PaymentStep.CardComplet;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
    }
    
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream) => stream.Step = PaymentStep;

}

public record CardProcessFailed : IPaymentEventStream
{
    public CardProcessFailed(Guid idPayment, string message)
    {        
        PaymentStep = PaymentStep.CardFail;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
        Message = message;
    }
    
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public string Message { get; init; }

    public void Process(PaymentStream stream)
    {
        stream.Step = PaymentStep;
        stream.Message = Message;
    }

}