using Domain.Core.Observability;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();  
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .Build();

        return Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration(appConfig =>
             {
                 var configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", false, true)
                     .Build();
                 appConfig.AddConfiguration(configuration);
             })            
           .AddLogginSerilog("Notification", config)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddTracing("Notification", hostContext.Configuration);
               services.ConfigureLogging();
               services.AddHostedService<HandlerNotification>();
           });
    }
       
}