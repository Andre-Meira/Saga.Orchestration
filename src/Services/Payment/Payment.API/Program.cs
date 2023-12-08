using Domain.Core.Observability;
using MassTransit;
using Payment.Core;
using Payment.Core.Machine;
using Payment.Core.Machine.Activitys.BankActivity;
using Payment.Core.Machine.Activitys.CardActivity;
using Payment.Core.Orchestration;
using Payment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string nameService = "Payment.API";

builder.Host.AddLogginSerilog(nameService);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureLogging();
builder.Services.AddTracing(nameService,builder.Configuration);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddWorkerService(builder.Configuration);


builder.Services.AddMassTransit(e =>
{
    e.SetKebabCaseEndpointNameFormatter();
    
    e.AddConsumer<PaymentWoker>(typeof(OrchestrationWokerDefinition));
    e.AddSagaStateMachine<PaymenteStateMachine, PaymentState>().MongoDbRepository(r =>
    {
        r.Connection = "mongodb://root:root@localhost:27017";
        r.DatabaseName = "Payment";
        r.CollectionName = "Order";        
    });

    e.AddActivitiesFromNamespaceContaining<CardProcessActivity>();
    e.AddActivitiesFromNamespaceContaining<BankProcessActivity>();

    e.UsingRabbitMq((context, cfg) =>
    {                

        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
