using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class DummyEvent : DomainEvent
    {
        public DummyEvent() : base("Dummy.Topic")
        {
        }

        public string RandomData { get; set; }
    }
}
