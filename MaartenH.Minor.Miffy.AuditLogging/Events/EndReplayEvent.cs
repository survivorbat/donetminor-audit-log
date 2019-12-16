using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    public class EndReplayEvent : DomainEvent
    {
        public EndReplayEvent(Guid processId) : base(ReplayTopicNames.ReplayEndEventTopic, processId)
        {
        }
    }
}
