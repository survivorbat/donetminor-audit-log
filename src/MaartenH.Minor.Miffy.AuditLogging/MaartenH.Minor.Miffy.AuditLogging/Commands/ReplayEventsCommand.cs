using System;
using Minor.Miffy.MicroServices.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    public class ReplayEventsCommand : DomainCommand
    {
        public ReplayEventsCommand() : base("auditlog.replay")
        {
        }

        public long? FromTimeStamp { get; set; }
        public long? ToTimeStamp { get; set; }
        public string EventType { get; set; }
        public string Topic { get; set; }
    }
}
