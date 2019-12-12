using System;
using System.Linq;
using System.Reflection;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Host
{
    public class MicroserviceReplayListener : MicroserviceListener
    {
        public void ApplyListenerSettings(MicroserviceListener listener)
        {
            Queue = $"replay_{listener.Queue}";
            TopicExpressions = listener.TopicExpressions.Select(e => $"replay_{e}");
        }

        public bool Equals(MicroserviceListener other)
        {
            return Queue == other.Queue;
        }
    }
}
