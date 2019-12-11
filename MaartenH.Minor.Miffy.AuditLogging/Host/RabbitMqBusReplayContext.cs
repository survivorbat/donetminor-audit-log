using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class RabbitMqBusReplayContext : RabbitMqBusContext
    {
        public RabbitMqBusReplayContext(IConnection connection, string exchangeName) : base(connection, exchangeName)
        {
        }
    }
}
