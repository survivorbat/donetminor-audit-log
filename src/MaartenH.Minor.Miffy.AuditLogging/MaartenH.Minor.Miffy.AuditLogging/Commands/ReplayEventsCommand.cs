using System;
using Minor.Miffy.MicroServices.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    public class ReplayEventsCommand : DomainCommand
    {
        public ReplayEventsCommand() : base("auditlog.replay")
        {
        }
    }
}
