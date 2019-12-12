﻿using System.Threading;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace ExampleService
{
    /// <summary>
    /// An example program that spams events to an auditlogger
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Entrypoint
        /// </summary>
        static void Main(string[] args)
        {
            /**
             * Logging is important, so first set up a logger factory
             */
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Information);
            });

            /**
             * Make sure the libraries use the proper logger factories
             */
            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            /**
             * Set up a context using environment variables
             */
            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            /**
             * Build a host using the loggerfactory, context and register any event listeners in the package.
             */
            using var hostBuilder = new MicroserviceReplayHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .WithBusContext(context)
                .UseConventions();

            /**
             * Now create the host and start it
             */
            using var host = hostBuilder.CreateHost();
            host.Start();

            /**
             * Start spamming events to the auditlogger as a demonstration
             */
            StartSpammingEvents(context);
        }

        /// <summary>
        /// Start spamming events to demonstrate how the AuditLogger works
        /// </summary>
        private static void StartSpammingEvents(IBusContext<IConnection> context)
        {
            /**
             * Create an event publisher
             */
            IEventPublisher eventPublisher = new EventPublisher(context);

            /**
             * Generate a random event and publish it, then wait 4000 seconds to repeat it
             */
            while (true)
            {
                DomainEvent domainEvent = ExampleData.GenerateRandomEvent();
                eventPublisher.Publish(domainEvent);
                Thread.Sleep(4000);
            }
        }
    }
}
