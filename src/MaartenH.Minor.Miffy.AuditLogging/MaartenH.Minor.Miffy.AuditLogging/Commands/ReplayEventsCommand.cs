using System;
using Minor.Miffy.MicroServices.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    public class ReplayEventsCommand : DomainCommand
    {
        public string ReplyQueue { get; set; }

        public ReplayEventsCommand() : base("auditlog.replay")
        {
        }
    }
}
