using System.Collections.Generic;
using Minor.Miffy.MicroServices.Host;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public interface IMicroserviceReplayHost : IMicroserviceHost
    {
        /// <summary>
        /// List of listeners specificly for replaying events
        /// </summary>
        IEnumerable<MicroserviceReplayListener> ReplayListeners { get; }

        /// <summary>
        /// Whether the listener is replaying or not
        /// </summary>
        bool IsReplaying { get; }

        /// <summary>
        /// Start replay listeners
        /// </summary>
        void StartReplay();

        /// <summary>
        /// Stop replay listeners
        /// </summary>
        void StopReplay();
    }
}
