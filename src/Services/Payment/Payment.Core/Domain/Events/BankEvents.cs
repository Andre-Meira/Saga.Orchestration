using Domain.Core;

namespace Payment.Core.Domain.Events;

public record BankProcessInitialized : IPaymentEventStream
{
    public BankProcessInitialized(Guid idPayment)
    {        
        PaymentStep = PaymentStep.BankProcessing;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
    }
    
    public PaymentStep PaymentStep { get; init; }
    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.Step is not PaymentStep.CardComplet)
        {
            throw new DomainException("Não é possivel iniciar ate o processo do cartão termina.");
        }

        stream.Step = PaymentStep;
        stream.Status = Status.Process;
    }
}

public record BankProcessCompleted : IPaymentEventStream
{
    public BankProcessCompleted(Guid idPayment)
    {        
        PaymentStep = PaymentStep.BankComplet;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
    }
    
    public PaymentStep PaymentStep { get; init; }
    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream) => stream.Step = PaymentStep;

}

public record BankProcessFailed : IPaymentEventStream
{
    public BankProcessFailed(Guid idPayment, string message)
    {        
        PaymentStep = PaymentStep.BankFail;
        DataProcessed = DateTime.Now;        
        IdCorrelation = idPayment;
        Message = message;
    }
    
    public PaymentStep PaymentStep { get; init; }
    public string Message { get; init; }

    public DateTime DataProcessed { get; init; }   
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        stream.Step = PaymentStep;
        stream.Message = Message;
    }
}