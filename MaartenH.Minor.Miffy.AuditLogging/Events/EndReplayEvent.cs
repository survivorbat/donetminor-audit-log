using System;
using System.Diagnostics.CodeAnalysis;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    /// <summary>
    /// Empty event that is sent to indicate the end of a replay
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EndReplayEvent : DomainEvent
    {
        /// <summary>
        /// Instantiate a replayend event with a process id
        /// </summary>
        public EndReplayEvent(Guid processId) : base(ReplayTopicNames.ReplayEndEventTopic, processId)
        {
        }
    }
}
