using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    public class StartReplayEvent : DomainEvent
    {
        public StartReplayEvent(Guid processId) : base(ReplayTopicNames.ReplayStartEventTopic, processId)
        {
        }
    }
}
