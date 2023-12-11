using Domain.Core;

namespace Payment.Core.Domain.Events;


public record PaymentInitialized : IPaymentEventStream
{
    public PaymentInitialized(Guid idPayment, Guid payer,
        Guid payee, decimal value)
    {
        IdPayment = idPayment;
        Payer = payer;
        Payee = payee;
        Value = value;
        Status = Status.Process;
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
        PaymentStep = PaymentStep.Process;
    }

    public Guid IdPayment { get; init; }
    public Guid Payer { get; init; }
    public Guid Payee { get; init; }
    public decimal Value { get; init; }
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }

    public DateTime DataProcessed { get; init; }
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.IdPayment.Equals(Guid.Empty) == false)
            throw new DomainException("O pagamento já foi iniciado.");

        stream.IdPayment = IdPayment;
        stream.Payee = Payee;
        stream.Value = Value;
        stream.Payeer = Payer;
        stream.Step = PaymentStep;
        stream.Date = DataProcessed;
    }
}


public record PaymentCompleted : IPaymentEventStream
{
    public PaymentCompleted(Guid idPayment)
    {        
        Status = Status.Complet;        
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
        PaymentStep = PaymentStep.Complet;
    }
    
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }
    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.Step is not PaymentStep.Complet)
            throw new DomainException("Não é possivel finalizar o pagamento em quanto a banco não termina.");

        stream.Step = PaymentStep;  
        stream.Status = Status;
    }
}


public record PaymentReversed : IPaymentEventStream
{
    public PaymentReversed(Guid idPayment, string mensagem)
    {        
        Mensagem = mensagem;
        Status = Status.Reversal;
        PaymentStep = PaymentStep.Reversal;

        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }
    
    public string Mensagem { get; init; }
    public Status Status { get; init; }
    public PaymentStep PaymentStep { get; init; }
    public DateTime DataProcessed { get; init; }
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.IsReversal == false)
            throw new Exception("O pagamento não está concluido.");

        stream.Step = PaymentStep;  
        stream.Status = Status;
        stream.Message = Mensagem;
    }
}
