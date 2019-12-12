using System;
using System.Diagnostics.CodeAnalysis;
using Minor.Miffy.MicroServices.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    /// <summary>
    /// Command that triggers the auditlogger to spew out historical events
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReplayEventsCommand : DomainCommand
    {
        /// <summary>
        /// Initializer tha created a new command with a set
        /// </summary>
        public ReplayEventsCommand(Guid processId) : base("auditlog.replay", processId)
        {
        }

        /// <summary>
        /// Timestamp from which to begin spewing out historical events
        /// </summary>
        public long? FromTimeStamp { get; set; }

        /// <summary>
        /// Timestamp at which to stop spewing out historical events
        /// </summary>
        public long ToTimeStamp { get; set; }

        /// <summary>
        /// The type of events that are desired
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Optinnal topic of all the events that you want to have returned
        /// </summary>
        public string Topic { get; set; }
    }
}
