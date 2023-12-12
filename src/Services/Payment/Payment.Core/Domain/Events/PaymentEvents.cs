using Domain.Core;
using Grpc.Core;

namespace Payment.Core.Domain.Events;


public record PaymentProcessInitialized : IPaymentEventStream
{
    public PaymentProcessInitialized(Guid idPayment, Guid payer,
        Guid payee, decimal value)
    {
        IdPayment = idPayment;
        Payer = payer;
        Payee = payee;
        Value = value;
        Status = Status.Process;
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;        
    }

    public Guid IdPayment { get; init; }
    public Guid Payer { get; init; }
    public Guid Payee { get; init; }
    public decimal Value { get; init; }
    public Status Status { get; init; }    

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
        stream.Date = DataProcessed;
    }
}


public record PaymentProcessCompleted : IPaymentEventStream
{
    public PaymentProcessCompleted(Guid idPayment, string message)
    {        
        Status = Status.Complet;        
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
        Message = message;
    }
        
    public Status Status { get; init; }    
    public string Message { get; init; }    

    public DateTime DataProcessed { get; init; }    
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.Status is not Status.Complet)
            throw new DomainException("Não é possivel finalizar o pagamento em quanto a banco não termina.");
        
        stream.Status = Status;
        stream.Message = Message;   
    }
}

public record PaymentProcessFailed : IPaymentEventStream
{

    public PaymentProcessFailed(Guid idPayment, string message)
    {
        Status = Status.Fail;
        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
        Message = message;
    }

    public Status Status { get; init; } 
    public string Message { get; init; }

    public Guid IdCorrelation { get ; init ; }
    public DateTime DataProcessed { get ; init ; }

    public void Process(PaymentStream stream)
    {
        stream.Status = Status;
        stream.Message = Message;
    }
}



public record PaymentProcessReversed : IPaymentEventStream
{
    public PaymentProcessReversed(Guid idPayment, string mensagem)
    {        
        Mensagem = mensagem;
        Status = Status.Reversal;        

        DataProcessed = DateTime.Now;
        IdCorrelation = idPayment;
    }
    
    public string Mensagem { get; init; }
    public Status Status { get; init; }    
    public DateTime DataProcessed { get; init; }
    public Guid IdCorrelation { get; init; }

    public void Process(PaymentStream stream)
    {
        if (stream.IsReversal == false)
            throw new Exception("O pagamento não está concluido.");
        
        stream.Status = Status;
        stream.Message = Mensagem;
    }
}
