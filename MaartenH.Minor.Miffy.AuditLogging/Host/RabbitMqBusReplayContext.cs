using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqBusReplayContext : RabbitMqBusContext
    {
        public string ReplayExchangePrefix { get; }

        public RabbitMqBusReplayContext(IConnection connection, string exchangeName, string replayExchangePrefix)
            : base(connection, exchangeName)
        {
            ReplayExchangePrefix = replayExchangePrefix;
        }
    }
}
