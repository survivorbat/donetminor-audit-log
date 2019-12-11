using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqReplayContextBuilder : RabbitMqContextBuilder
    {
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

            return new RabbitMqBusReplayContext(connection, ExchangeName);
        }
    }
}
