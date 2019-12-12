using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MaartenH.Minor.Miffy.AuditLogging.Events;
using MaartenH.Minor.Miffy.AuditLogging.Exceptions;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayHostBuilder : MicroserviceHostBuilder
    {
        /// <summary>
        /// A list of listeners that are exclusive for replaying events
        /// </summary>
        protected List<MicroserviceReplayListener> ReplayListeners = new List<MicroserviceReplayListener>();

        /// <summary>
        /// Analyze a type and register its methods as an event, command or replay listener
        /// </summary>
        /// <param name="type"></param>
        protected override void RegisterListener(TypeInfo type)
        {
            Logger.LogTrace($"Retrieving relevant methods from type {type.Name}");

            foreach (MethodInfo methodInfo in GetRelevantMethods(type))
            {
                string eventQueueName = methodInfo.GetCustomAttribute<EventListenerAttribute>()?.QueueName;
                string replayEventQueueName = methodInfo.GetCustomAttribute<ReplayEventListenerAttribute>()?.QueueName;
                string commandQueueName = methodInfo.GetCustomAttribute<CommandListenerAttribute>()?.QueueName;

                if (eventQueueName != null)
                {
                    RegisterEventListener(type, methodInfo, eventQueueName);
                }
                else if (commandQueueName != null)
                {
                    RegisterCommandListener(type, methodInfo, commandQueueName);
                }
                else if (replayEventQueueName != null)
                {
                    RegisterReplayEventListener(type, methodInfo, replayEventQueueName);
                }
                else
                {
                    Logger.LogTrace($"Method {methodInfo.Name} does not contain listener attributes.");
                }
            }
        }

        /// <summary>
        /// Register a listener for a specific event with topic expressions
        /// </summary>
        protected virtual void RegisterReplayEventListener(TypeInfo type, MethodInfo method, string queueName)
        {
            Logger.LogDebug($"Evaluating parameter type {type.Name} of replay method {method.Name}");
            Type parameterType = method.GetParameters().FirstOrDefault()?.ParameterType;

            ReplayListeners.Add(new MicroserviceReplayListener
            {
                Queue = queueName,
                Callback = message =>
                {
                    Logger.LogDebug($"Attempting to instantiate type {type.Name} in replay queue of queue {queueName}");
                    object instance = InstantiatePopulatedType(type);

                    Logger.LogTrace($"Retrieving string data from message with id {message.CorrelationId}");
                    string text = Encoding.Unicode.GetString(message.Body);

                    if (parameterType == StringType)
                    {
                        Logger.LogTrace($"Parameter type is a string, invoking method for message {message.CorrelationId} with body {text}");
                        method.Invoke(instance, new object[] {text});
                        return;
                    }

                    Logger.LogTrace($"Deserialized object from message with id {message.CorrelationId} and body {text}");
                    object jsonObject = JsonConvert.DeserializeObject(text, parameterType);

                    Logger.LogTrace($"Invoking method {method.Name} with message id {message.CorrelationId} and instance of type {type.Name} with data {text}");
                    method.Invoke(instance, new[] {jsonObject});
                }
            });
        }

        /// <summary>
        /// Validate if all listeners are properly registered, if a listener
        /// is properly setup, copy its non-replay counterpart's settings over to the
        /// replay listener.
        /// </summary>
        protected void ValidateReplayListeners()
        {
            if (EventListeners.Count != ReplayListeners.Count)
            {
                Logger.LogCritical("Amount of Event listeners is not equal to the amount of Replay listeners");

                throw new ListenerValidationException(
                    $"There are {EventListeners.Count} event listeners but {ReplayListeners.Count} replay listeners");
            }

            foreach (MicroserviceReplayListener listener in ReplayListeners)
            {
                MicroserviceListener normalListener = EventListeners.Find(e => listener.Equals(e));

                if (normalListener == null)
                {
                    Logger.LogCritical($"ReplayListener of {listener.Queue} has no non-replay counterpart.");
                    throw new ListenerValidationException($"ReplayListener of {listener.Queue} has no non-replay counterpart.");
                }

                Logger.LogDebug($"Applying configuration of listener for queue {listener.Queue} to replay listener");
                listener.ApplyListenerSettings(normalListener);
            }
        }

        /// <summary>
        /// Create a microservice replay host
        /// </summary>
        public override MicroserviceHost CreateHost()
        {
            ValidateReplayListeners();
            var logger = LoggerFactory.CreateLogger<MicroserviceReplayHost>();
            return new MicroserviceReplayHost(Context, ReplayListeners, EventListeners, CommandListeners, logger);
        }
    }
}
