using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Debug);
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

                })
                .UseConventions()
                .CreateHost();

            host.Start();

            // Keep app running
            new ManualResetEvent(false).WaitOne();
        }
    }
}
