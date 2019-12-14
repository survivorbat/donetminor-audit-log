namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    /// <summary>
    /// A special publisher that is used to initiate a replay
    /// </summary>
    public interface IReplayCommandPublisher
    {
        /// <summary>
        /// Publish a replay event command and initiate a replay
        /// </summary>
        void Initiate(ReplayEventsCommand command);
    }
}
