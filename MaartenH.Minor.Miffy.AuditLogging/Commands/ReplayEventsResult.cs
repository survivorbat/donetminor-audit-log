using System.Diagnostics.CodeAnalysis;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    /// <summary>
    /// A result object that is returned after a ReplayEventsCommand is being handled
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReplayEventsResult
    {
        /// <summary>
        /// An indicator that tells the using client how many events
        /// were found in the Audit Logger
        /// </summary>
        public int AmountOfEvents { get; set; }
    }
}
