using ftrip.io.framework.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ftrip.io.email_service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilogLogging((hostingContext) =>
                {
                    return new LoggingOptions()
                    {
                        ApplicationName = hostingContext.HostingEnvironment.ApplicationName,
                        ApplicationLabel = "email",
                        ClientIdAttribute = "X-Forwarded-For",
                        GrafanaLokiUrl = Environment.GetEnvironmentVariable("GRAFANA_LOKI_URL"),
                        ConsoleOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] - {Message:lj}{NewLine}"
                    };
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}