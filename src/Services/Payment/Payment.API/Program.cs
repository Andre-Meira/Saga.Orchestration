using Domain.Core.Observability;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Core;
using Payment.Core.Consumers;
using Payment.Core.Machine;
using Payment.Core.Machine.Activitys.BankActivity;
using Payment.Core.Machine.Activitys.CardActivity;
using Payment.Core.Notifications;
using Payment.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
string nameService = "Payment.API";

builder.Host.AddLogginSerilog(nameService, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureLogging();
builder.Services.AddTracing(nameService,builder.Configuration);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddWorkerService(builder.Configuration);
builder.Services.AddSignalR();


builder.Services.AddMassTransit(e =>
{
    e.SetKebabCaseEndpointNameFormatter();
    
    e.AddConsumer<PaymentWoker>(typeof(OrchestrationWokerDefinition));
    e.AddConsumer<PaymentNotification>(typeof(PaymentNotificationWokerDefinition));

    e.AddSagaStateMachine<PaymenteStateMachine, PaymentState>().MongoDbRepository(r =>
    {
        r.Connection = "mongodb://root:root@localhost:27017";
        r.DatabaseName = "Payment";
        r.CollectionName = "PaymentMachine";        
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

app.MapHub<PaymentHub>("/payment-notification");

app.Run();
