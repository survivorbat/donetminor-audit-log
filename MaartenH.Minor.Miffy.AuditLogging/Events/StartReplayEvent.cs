using System;
using System.Diagnostics.CodeAnalysis;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    /// <summary>
    /// Empty event that is sent to indicate the start of a replay
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StartReplayEvent : DomainEvent
    {
        /// <summary>
        /// Instantiate a replaystart event with a process id
        /// </summary>
        public StartReplayEvent(Guid processId) : base(ReplayTopicNames.ReplayStartEventTopic, processId)
        {
        }
    }
}
