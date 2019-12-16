using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;

namespace MaartenH.Minor.Miffy.AuditLogging.Server
{
    /// <summary>
    /// This class is tested in an integration test
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                string logLevelString = Environment.GetEnvironmentVariable(EnvVarNames.LogLevel) ?? "Information";
                LogLevel logLevel = Enum.Parse<LogLevel>(logLevelString);
                configure.AddConsole().SetMinimumLevel(logLevel);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            using var host = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .WithBusContext(context)
                .RegisterDependencies(services =>
                {
                    services.AddDbContext<AuditLogContext>(config =>
                    {
                        config.UseMySql(Environment.GetEnvironmentVariable(EnvVarNames.DatabaseConnectionString));
                        config.UseLoggerFactory(loggerFactory);
                    }, ServiceLifetime.Singleton);

                    services.AddTransient<IAuditLogItemRepository, AuditLogItemRepository>();
                    services.AddTransient<IEventPublisher, EventPublisher>();

                    using var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var auditLogContext = serviceScope.ServiceProvider.GetService<AuditLogContext>();
                    auditLogContext.Database.Migrate();
                })
                .UseConventions()
                .CreateHost();

            host.Start();

            // Keep app running
            new ManualResetEvent(false).WaitOne();
        }
    }
}
