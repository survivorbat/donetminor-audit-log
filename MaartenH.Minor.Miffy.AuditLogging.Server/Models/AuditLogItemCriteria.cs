using System.Collections.Generic;
using MaartenH.Minor.Miffy.AuditLogging.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    /// <summary>
    /// Criteria available to query logitems with
    /// </summary>
    public class AuditLogItemCriteria
    {
        /// <summary>
        /// Whether to allow auditlogger events to be published in the replay
        /// </summary>
        public bool AllowMetaEvents { get; set; }

        /// <summary>
        /// Types of events
        /// </summary>
        public IEnumerable<string> Types { get; set; } = new List<string>();

        /// <summary>
        /// Event topics
        /// </summary>
        public IEnumerable<string> Topics { get; set; } = new List<string>();

        /// <summary>
        /// Timestamp from when to look
        /// </summary>
        public long FromTimeStamp { get; set; }

        /// <summary>
        /// Timestamp to which we should replay
        /// </summary>
        public long ToTimeStamp { get; set; }

        /// <summary>
        /// Cast an incoming command to a criteria object
        /// </summary>
        public static explicit operator AuditLogItemCriteria(ReplayEventsCommand command)
        {
            return new AuditLogItemCriteria
            {
                Topics = command.Topics,
                Types = command.Types,
                FromTimeStamp = command.FromTimeStamp,
                ToTimeStamp = command.ToTimeStamp,
                AllowMetaEvents = command.AllowMetaEvents
            };
        }
    }
}
