using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class PersonAddedEvent : DomainEvent
    {
        public PersonAddedEvent() : base(TopicNames.PersonAddedTopic)
        {
        }

        public Person Person { get; set; }
    }
}
