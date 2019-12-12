using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Host;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayHostBuilder : MicroserviceHostBuilder
    {
        /// <summary>
        /// Create a microservice replay host
        /// </summary>
        public override MicroserviceHost CreateHost()
        {
            var logger = LoggerFactory.CreateLogger<MicroserviceReplayHost>();
            return new MicroserviceReplayHost(Context, EventListeners, CommandListeners, logger);
        }
    }
}
