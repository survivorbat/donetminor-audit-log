using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.Events
{
    public class DummyEvent : DomainEvent
    {
        public DummyEvent(string topic) : base(topic)
        {
        }
    }
}
