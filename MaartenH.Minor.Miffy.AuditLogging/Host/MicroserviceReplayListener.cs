using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    /// <summary>
    /// Special listener specifically for replay event listeners.
    ///
    /// It contains a special ApplyListenerSettings method to
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
            Queue = $"{ReplayTopicNames.ReplayEventTopicPrefix}{listener.Queue}";
            TopicExpressions = listener.TopicExpressions.Select(e => $"{ReplayTopicNames.ReplayEventTopicPrefix}{e}");
        }
    }
}
