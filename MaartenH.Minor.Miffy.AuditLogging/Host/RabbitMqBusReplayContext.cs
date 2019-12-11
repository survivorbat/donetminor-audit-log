using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqBusReplayContext : RabbitMqBusContext
    {
        protected string ReplayExchangeName { get; }

        public RabbitMqBusReplayContext(IConnection connection,
            string exchangeName, string replayExchangeName) : base(connection, exchangeName)
        {
            ReplayExchangeName = replayExchangeName;
        }
    }
}
