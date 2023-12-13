using Domain.Core.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Payment.Application.Consumers;
using Payment.Application.Machine;
using Payment.Application.Machine.Activitys.BankActivity;
using Payment.Application.Machine.Activitys.CardActivity;

namespace Payment.API.Configurations;

public static class BusConfiguration
{
    public static IServiceCollection AddBus(this IServiceCollection services, 
        IConfiguration configuration)
    {
        BusOptions busOptions = configuration.GetSection(BusOptions.Key).Get<BusOptions>();
        MongoOptions mongoOptions = configuration.GetSection(MongoOptions.Key).Get<MongoOptions>();

        services.AddMassTransit(e =>
        {
            e.SetKebabCaseEndpointNameFormatter();

            e.AddConsumer<PaymentWoker>(typeof(OrchestrationWokerDefinition));
            e.AddConsumer<PaymentNotification>(typeof(PaymentNotificationWokerDefinition));

            e.AddSagaStateMachine<PaymenteStateMachine, PaymentState>()
                .MongoDbRepository(r =>
                {
                    r.Connection = mongoOptions.Connection;
                    r.DatabaseName = mongoOptions.Connection;
                    r.CollectionName = nameof(PaymenteStateMachine);
                });

            e.AddActivitiesFromNamespaceContaining<CardProcessActivity>();
            e.AddActivitiesFromNamespaceContaining<BankProcessActivity>();

            e.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(busOptions.Host, busOptions.VirtualHost, h =>
                {
                    h.Username(busOptions.UserName);
                    h.Password(busOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
