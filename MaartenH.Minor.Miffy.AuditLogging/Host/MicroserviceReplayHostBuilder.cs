using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Host;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayHostBuilder : MicroserviceHostBuilder
    {
        protected List<MicroserviceReplayListener> ReplayListeners = new List<MicroserviceReplayListener>();

        protected override void RegisterEventListener(TypeInfo type, MethodInfo method, string queueName)
        {
            base.RegisterEventListener(type, method, queueName);
            
            
        }

        /// <summary>
        /// Create a microservice replay host
        /// </summary>
        public override MicroserviceHost CreateHost()
        {
            var logger = LoggerFactory.CreateLogger<MicroserviceReplayHost>();
            return new MicroserviceReplayHost(Context, ReplayListeners, EventListeners, CommandListeners, logger);
        }
    }
}
