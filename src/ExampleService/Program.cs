using System;
using System.Text;
using System.Threading;
using ExampleService.Events;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
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
                configure.AddConsole().SetMinimumLevel(LogLevel.Information);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            IEventPublisher eventPublisher = new EventPublisher(context);

            while (true)
            {
                byte[] bytes = new byte[20];
                new Random().NextBytes(bytes);

                DummyEvent exampleEvent = new DummyEvent
                {
                    Data = Encoding.Unicode.GetString(bytes)
                };

                eventPublisher.Publish(exampleEvent);

                Thread.Sleep(2000);
            }
        }
    }
}
