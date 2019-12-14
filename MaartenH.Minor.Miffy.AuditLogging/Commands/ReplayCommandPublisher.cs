using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Commands;

namespace MaartenH.Minor.Miffy.AuditLogging.Commands
{
    public class ReplayCommandPublisher : IReplayCommandPublisher
    {
        /// <summary>
        /// Current host to replay
        /// </summary>
        protected IMicroserviceReplayHost Host { get; }

        /// <summary>
        /// Command publisher to publish commands to the bus
        /// </summary>
        protected ICommandPublisher CommandPublisher { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger<ReplayCommandPublisher> Logger { get; }

        /// <summary>
        /// Instantiate a replay command publisher used
        /// </summary>
        public ReplayCommandPublisher(IMicroserviceReplayHost host, ICommandPublisher commandPublisher, ILoggerFactory loggerFactory)
        {
            Host = host;
            CommandPublisher = commandPublisher;
            Logger = loggerFactory.CreateLogger<ReplayCommandPublisher>();
        }

        /// <summary>
        /// Publish a replay event command and initiate a replay
        /// </summary>
        public virtual void Initiate(ReplayEventsCommand command)
        {
            if (Host.IsListening && !Host.IsPaused)
            {
                Host.Pause();
            }

            Host.StartReplay();

            // TODO: Replay logic

            Host.StopReplay();

            if (Host.IsListening && Host.IsPaused)
            {
                Host.Resume();
            }
        }
    }
}
