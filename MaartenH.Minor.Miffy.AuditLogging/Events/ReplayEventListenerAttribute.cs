using System;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReplayEventListenerAttribute : Attribute
    {
        public string QueueName { get; }

        public ReplayEventListenerAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
