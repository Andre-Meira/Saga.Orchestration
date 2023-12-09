using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net.Http;

namespace Domain.Core.Observability;

public static class TraceConfiguration
{
    public static OpenTelemetryBuilder AddTracing(
        this IServiceCollection services,
        string nameService,
        IConfiguration configure)
    {
        return services.AddOpenTelemetry()
           .WithTracing(tracing =>
           {
               ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault()
                  .AddService(nameService,
                  serviceVersion: "1.0",
                  serviceInstanceId: Environment.MachineName);

               tracing.AddSource("MassTransit");

               string endpointOtlp = configure["EndpointOtlp"]!;

               tracing.AddAspNetCoreInstrumentation(asp =>
               {
                   asp.Filter = FilterReques;
                   asp.EnrichWithHttpRequest = (Activity activity, HttpRequest request) =>
                   {
                       activity.AddTag("TraceId", activity.TraceId);
                   };
               });

               tracing.AddHttpClientInstrumentation();


               tracing.AddOtlpExporter(e =>
               {
                   e.Endpoint = new Uri(endpointOtlp);
                   e.Protocol = OtlpExportProtocol.Grpc;
               });


               tracing.SetResourceBuilder(resourceBuilder);
           });
    }

    private static bool FilterReques(HttpContext http)
    {
        return !http.Request.Path.Value!.Contains("/swagger")
            & !http.Request.Path.Value!.Contains("_framework")
            & !http.Request.Path.Value!.Contains("_vs");
    }
}
