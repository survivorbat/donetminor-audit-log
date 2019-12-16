using System;
using System.Collections.Generic;
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
        /// Initialize a new ReplayEventsCommand with an empty process id
        /// </summary>
        public ReplayEventsCommand(long toTimeStamp) : this(toTimeStamp, Guid.Empty)
        {
        }

        /// <summary>
        /// Initialize a new ReplayEventsCommand with a given process id
        /// </summary>
        public ReplayEventsCommand(long toTimeStamp, Guid processId) : base("auditlog.replay", processId)
        {
            ToTimeStamp = toTimeStamp;
        }

        /// <summary>
        /// Timestamp from which to begin spewing out historical events
        /// </summary>
        public long? FromTimeStamp { get; set; }

        /// <summary>
        /// Timestamp at which to stop spewing out historical events
        /// </summary>
        public long ToTimeStamp { get; }

        /// <summary>
        /// The type of events that are desired
        /// </summary>
        public List<string> Types { get; set; }

        /// <summary>
        /// Optinnal topic of all the events that you want to have returned
        /// </summary>
        public List<string> Topics { get; set; }
    }
}
