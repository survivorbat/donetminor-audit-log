using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayHost : MicroserviceHost
    {
        /// <summary>
        /// Instantiate a replay host
        /// </summary>
        public MicroserviceReplayHost(IBusContext<IConnection> connection,
            IEnumerable<MicroserviceListener> listeners,
            IEnumerable<MicroserviceListener> replayListeners,
            IEnumerable<MicroserviceCommandListener> commandListeners,
            ILogger<MicroserviceHost> logger) : base(connection, listeners, commandListeners, logger)
        {
        }
    }
}
