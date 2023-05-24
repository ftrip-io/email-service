using ftrip.io.email_service.Installers;
using ftrip.io.framework.Correlation;
using ftrip.io.framework.email.Installers;
using ftrip.io.framework.ExceptionHandling.Extensions;
using ftrip.io.framework.HealthCheck;
using ftrip.io.framework.Installers;
using ftrip.io.framework.Mapping;
using ftrip.io.framework.messaging.Installers;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Installers;
using ftrip.io.framework.Tracing;
using ftrip.io.framework.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ftrip.io.email_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            InstallerCollection.With(
                new HealthCheckUIInstaller(services),
                new AutoMapperInstaller<Startup>(services),
                new FluentValidationInstaller<Startup>(services),
                new MongodbInstaller(services),
                new MongodbHealthCheckInstaller(services),
                new RabbitMQInstaller<Startup>(services, RabbitMQInstallerType.Consumer),
                new DependenciesIntaller(services),
                new EmailDispatcherInstaller<Startup>(services),
                new CorrelationInstaller(services),
                new TracingInstaller(services, (tracingSettings) =>
                {
                    tracingSettings.ApplicationLabel = "email";
                    tracingSettings.ApplicationVersion = GetType().Assembly.GetName().Version?.ToString() ?? "unknown";
                    tracingSettings.MachineName = Environment.MachineName;
                })
            ).Install();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCorrelation();
            app.UseFtripioGlobalExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseFtripioHealthCheckUI(Configuration.GetSection(nameof(HealthCheckUISettings)).Get<HealthCheckUISettings>());
        }
    }
}