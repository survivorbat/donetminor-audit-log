using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayHost : MicroserviceHost, IMicroserviceReplayHost
    {
        /// <summary>
        /// Listeners specifically for replaying events
        /// </summary>
        public IEnumerable<MicroserviceReplayListener> ReplayListeners { get; }

        /// <summary>
        /// Listener specifically to listen for the 'end' event
        /// </summary>
        public MicroserviceReplayListener EndListener { get; set; }

        /// <summary>
        /// Listener specifically to listen for the 'start' event
        /// </summary>
        public MicroserviceReplayListener StartListener { get; set; }

        /// <summary>
        /// Indicates whether the host is currently replaying or not
        /// </summary>
        public bool IsReplaying { get; protected set; }

        /// <summary>
        /// List of replay message receivers
        /// </summary>
        protected List<IMessageReceiver> ReplayMessageReceivers { get; } = new List<IMessageReceiver>();

        /// <summary>
        /// Instantiate a replay host
        /// </summary>
        public MicroserviceReplayHost(IBusContext<IConnection> connection,
            IEnumerable<MicroserviceListener> listeners,
            IEnumerable<MicroserviceReplayListener> replayListeners,
            IEnumerable<MicroserviceCommandListener> commandListeners,
            ILogger<MicroserviceHost> logger) : base(connection, listeners, commandListeners, logger)
        {
            ReplayListeners = replayListeners;
        }

        /// <summary>
        /// Start replay listeners
        /// </summary>
        public void StartReplay()
        {
            if (IsReplaying)
            {
                throw new BusConfigurationException("Attempted to replay the MicroserviceHost, but it is already replaying.");
            }

            Logger.LogInformation($"Starting replay with {ReplayListeners.Count()} replay listeners");

            IsReplaying = true;

            foreach (MicroserviceReplayListener callback in ReplayListeners.Append(StartListener).Append(EndListener))
            {
                Logger.LogInformation($"Registering replay queue {callback.Queue} with expressions {string.Join(", ", callback.TopicExpressions)}");

                IMessageReceiver receiver = Context.CreateMessageReceiver(callback.Queue, callback.TopicExpressions);
                receiver.StartReceivingMessages();
                receiver.StartHandlingMessages(callback.Callback);
                ReplayMessageReceivers.Add(receiver);
            }
        }

        /// <summary>
        /// Stop replay listeners
        /// </summary>
        public void StopReplay()
        {
            if (!IsReplaying)
            {
                throw new BusConfigurationException("Attempted to stop replaying the MicroserviceHost, but it is not replaying.");
            }

            Logger.LogInformation($"Stopping replay with {ReplayListeners.Count()} replay listeners");

            IsReplaying = false;

            ReplayMessageReceivers.ForEach(e => e.Dispose());
        }
    }
}
