using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqReplayContextBuilder : RabbitMqContextBuilder
    {
        protected string ReplayExchangeName { get; set; }

        /// <summary>
        /// Add a name for the replay exchange that will be used to replay events
        /// </summary>
        public RabbitMqReplayContextBuilder WithReplayExchangeName(string replayExchangeName)
        {
            ReplayExchangeName = replayExchangeName;
            return this;
        }

        /// <inheritdoc/>
        public override RabbitMqContextBuilder ReadFromEnvironmentVariables()
        {
            ReplayExchangeName = Environment.GetEnvironmentVariable(EnvNames.ReplayExchangeName) ??
                                 throw new BusConfigurationException($"{EnvNames.ReplayExchangeName} variable not set");

            return base.ReadFromEnvironmentVariables();
        }

        /// <summary>
        /// Create a RabbitMqBusReplayContext using a new connection, exchange and replay exchange
        /// </summary>
        public override IBusContext<IConnection> CreateContext(IConnectionFactory connectionFactory)
        {
            using IConnection connection = connectionFactory.CreateConnection();

            using (IModel model = connection.CreateModel())
            {
                model.ExchangeDeclare(ReplayExchangeName, ExchangeType.Direct);
                model.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            }

            return new RabbitMqBusReplayContext(connection, ExchangeName, ReplayExchangeName);
        }
    }
}
