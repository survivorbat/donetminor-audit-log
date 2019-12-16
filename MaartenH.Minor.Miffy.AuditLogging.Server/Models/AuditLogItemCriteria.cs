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
        /// Types of events
        /// </summary>
        public IEnumerable<string> Types { get; set; }

        /// <summary>
        /// Event topics
        /// </summary>
        public IEnumerable<string> Topics { get; set; }

        /// <summary>
        /// Timestamp from when to look
        /// </summary>
        public long? FromTimeStamp { get; set; }

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
                ToTimeStamp = command.ToTimeStamp
            };
        }
    }
}
