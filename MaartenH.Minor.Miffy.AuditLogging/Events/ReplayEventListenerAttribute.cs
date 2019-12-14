using System;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    /// <summary>
    /// Attribute to mark a method as a ReplayEventListener.
    ///
    /// The queuename given to the attribute must correspond to an existing EventListener queue
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ReplayEventListenerAttribute : Attribute
    {
        /// <summary>
        /// Name of the existing queue
        /// </summary>
        public string QueueName { get; }

        /// <summary>
        /// Instantiate a replayeventlistener that references an event listener by a queue name
        /// </summary>
        /// <param name="queueName">Name of the existing queue</param>
        public ReplayEventListenerAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
