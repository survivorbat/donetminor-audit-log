using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqReplayContextBuilder : RabbitMqContextBuilder
    {
        public string ReplayExchangePrefix { get; protected set; }

        /// <summary>
        /// Add a name for the replay exchange that will be used to replay events
        /// </summary>
        public RabbitMqReplayContextBuilder WithReplayExchangePrefix(string replayExchangePrefix)
        {
            ReplayExchangePrefix = replayExchangePrefix;
            return this;
        }

        /// <inheritdoc/>
        public override RabbitMqContextBuilder ReadFromEnvironmentVariables()
        {
            ReplayExchangePrefix = Environment.GetEnvironmentVariable(EnvVarNames.ReplayExchangeName) ??
                                 throw new BusConfigurationException($"{EnvVarNames.ReplayExchangeName} variable not set");

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
                model.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            }

            return new RabbitMqBusReplayContext(connection, ExchangeName, ReplayExchangePrefix);
        }
    }
}
