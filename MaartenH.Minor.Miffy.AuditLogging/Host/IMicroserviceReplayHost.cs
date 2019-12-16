using System.Collections.Generic;
using Minor.Miffy.MicroServices.Host;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public interface IMicroserviceReplayHost : IMicroserviceHost
    {
        /// <summary>
        /// List of listeners specifically for replaying events
        /// </summary>
        IEnumerable<MicroserviceReplayListener> ReplayListeners { get; }

        /// <summary>
        /// Listener that is temporarily used to see when a replay is starting
        /// </summary>
        MicroserviceReplayListener StartListener { get; set; }

        /// <summary>
        /// Listener that is temporarily used to see when a replay stops
        /// </summary>
        MicroserviceReplayListener EndListener { get; set; }

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
