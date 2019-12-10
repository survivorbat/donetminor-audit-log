using System.Threading;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.RabbitMQBus;

namespace ExampleService
{
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Error);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            IEventPublisher eventPublisher = new EventPublisher(context);

            while (true)
            {
                DomainEvent domainEvent = SeedData.GenerateRandomEvent();
                eventPublisher.Publish(domainEvent);
                Thread.Sleep(4000);
            }
        }
    }
}
