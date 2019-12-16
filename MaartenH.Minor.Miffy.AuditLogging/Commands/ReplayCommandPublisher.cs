using System;
using System.Threading;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
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
        protected virtual IMicroserviceReplayHost Host { get; }

        /// <summary>
        /// Command publisher to publish commands to the bus
        /// </summary>
        protected virtual ICommandPublisher CommandPublisher { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected virtual ILogger<ReplayCommandPublisher> Logger { get; }

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
                                  $"Topic {string.Join(',', command.Topics)}, " +
                                  $"Types {string.Join(',', command.Types)}, " +
                                  $"DestinationQueue {command.DestinationQueue}. " +
                                  $"Process ID {command.ProcessId} and Timestamp {command.Timestamp}");

            if (Host.IsListening && !Host.IsPaused)
            {
                Logger.LogDebug("Pausing host");
                Host.Pause();
            }

            Logger.LogTrace("Setting up manual reset events");
            ManualResetEvent startResetEvent = new ManualResetEvent(false);
            ManualResetEvent endResetEvent = new ManualResetEvent(false);

            Logger.LogTrace("Setting up random queue names");
            string startQueue = Guid.NewGuid().ToString();
            string endQueue = Guid.NewGuid().ToString();

            Logger.LogDebug($"Setting up replay startlistener with topic {ReplayTopicNames.ReplayStartEventTopic} " +
                            $"and queue {startQueue}");

            MicroserviceReplayListener startListener = new MicroserviceReplayListener
            {
                Callback = callbackResult => startResetEvent.Set(),
                Queue = startQueue,
                TopicExpressions = new [] { ReplayTopicNames.ReplayStartEventTopic }
            };

            Logger.LogDebug($"Setting up replay endlistener with topic {ReplayTopicNames.ReplayEndEventTopic} " +
                            $"and queue {endQueue}");

            MicroserviceReplayListener endListener = new MicroserviceReplayListener
            {
                Callback = callbackResult => endResetEvent.Set(),
                Queue = endQueue,
                TopicExpressions = new [] { ReplayTopicNames.ReplayEndEventTopic  }
            };

            Logger.LogTrace("Setting up start and end listeners in host");
            Host.StartListener = startListener;
            Host.EndListener = endListener;

            Logger.LogTrace("Starting replay");
            Host.StartReplay();

            Logger.LogDebug("Publishing replay command to auditlogger");
            ReplayEventsResult result = CommandPublisher.PublishAsync<ReplayEventsResult>(command).Result;

            Logger.LogInformation($"{result.AmountOfEvents} events will be replayed");
            Logger.LogDebug("Awaiting start event");

            startResetEvent.WaitOne();

            Logger.LogInformation("Start event received, awaiting end event");

            endResetEvent.WaitOne();

            Logger.LogDebug("End event received, ending replay");
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
