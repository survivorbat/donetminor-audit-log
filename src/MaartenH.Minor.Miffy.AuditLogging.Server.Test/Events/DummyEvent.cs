using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Events
{
    public class DummyEvent : DomainEvent
    {
        public DummyEvent(string topic) : base(topic)
        {
        }
    }
}
