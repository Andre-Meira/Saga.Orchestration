using Domain.Core.Observability;
using MassTransit;
using Payment.Core;
using Payment.Core.Bank;
using Payment.Core.Orchestration;
using Payment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTracing("Payment.API",builder.Configuration);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddWorkerService(builder.Configuration);

builder.Services.AddMassTransit(e =>
{
    e.SetKebabCaseEndpointNameFormatter();

    e.AddConsumer<OrchestrationWoker>(typeof(OrchestrationWokerDefinition));
    e.AddConsumer<CardWorker>(typeof(CardWokerDefinition));
    e.AddConsumer<BankWorker>(typeof(BankWorkerDefinition));

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
