using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Events
{
    public class DummyEvent : DomainEvent
    {
        public string Text { get; set; }

        public DummyEvent(string topic) : base(topic)
        {
        }
    }
}
