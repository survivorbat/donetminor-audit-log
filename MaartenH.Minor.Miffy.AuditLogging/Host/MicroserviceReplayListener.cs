using System.Collections.Generic;
using System.Linq;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    /// <summary>
    /// Special listener specifically for replay event listeners.
    ///
    /// It contains a special Equals method and an ApplyListenerSettings method to
    /// copy over configuration
    /// </summary>
    public class MicroserviceReplayListener : MicroserviceListener
    {
        /// <summary>
        /// Apply the settings of a MicroserviceListener to a ReplayListener
        ///
        /// This allows us to keep the configuration of a replaylistener at 0 and just copy
        /// everything from an existing listener
        /// </summary>
        public void ApplyListenerSettings(MicroserviceListener listener)
        {
            Queue = $"replay_{listener.Queue}";
            TopicExpressions = listener.TopicExpressions.Select(e => $"replay_{e}");
        }

        /// <summary>
        /// Two event/reply listeners with the same name are considered equal
        /// </summary>
        public bool Equals(MicroserviceListener other)
        {
            return Queue == other.Queue;
        }
    }
}
