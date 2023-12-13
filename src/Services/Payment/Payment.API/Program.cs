using Domain.Core.Observability;
using Payment.Application.Notifications;
using Payment.Infrastructure;
using Payment.Application;
using Domain.Core.Options;
using Payment.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
string nameService = "Payment.API";
builder.Host.AddLogginSerilog(nameService, builder.Configuration);

builder.Services.AddOptions();
builder.Services.Configure<BusOptions>(builder.Configuration.GetSection(BusOptions.Key));
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection(MongoOptions.Key));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureLogging();
builder.Services.AddTracing(nameService,builder.Configuration);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddApplicationsService(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.AddBus(builder.Configuration);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapHub<PaymentHub>("/payment-notification");

app.Run();
