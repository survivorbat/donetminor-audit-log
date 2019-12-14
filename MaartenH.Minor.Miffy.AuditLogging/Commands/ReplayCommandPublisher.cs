using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        public ReplayCommandPublisher(IMicroserviceReplayHost host, ICommandPublisher commandPublisher, ILoggerFactory loggerFactory = null)
        {
            loggerFactory ??= new NullLoggerFactory();

            Host = host;
            CommandPublisher = commandPublisher;
            Logger = loggerFactory.CreateLogger<ReplayCommandPublisher>();
        }

        /// <summary>
        /// Publish a replay event command and initiate a replay
        /// </summary>
        public virtual void Initiate(ReplayEventsCommand command)
        {
            Logger.LogInformation($"Initiating replay from {command.FromTimeStamp} to {command.ToTimeStamp}, " +
                                  $"Topic {command.ToTimeStamp}, " +
                                  $"Type {command.Type}, " +
                                  $"DestinationQueue {command.DestinationQueue}. " +
                                  $"Process ID {command.ProcessId} and Timestamp {command.Timestamp}");

            if (Host.IsListening && !Host.IsPaused)
            {
                Logger.LogDebug("Pausing host");
                Host.Pause();
            }

            Host.StartReplay();

            // TODO: Replay logic

            Logger.LogDebug("Stopping replay");
            Host.StopReplay();

            if (!Host.IsListening)
            {
                return;
            }

            Logger.LogDebug("Resuming host");
            Host.Resume();
        }
    }
}
