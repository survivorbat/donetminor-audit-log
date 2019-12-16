using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class PersonDeletedEvent : DomainEvent
    {
        public PersonDeletedEvent() : base(TopicNames.PersonDeletedTopic)
        {
        }

        public Person Person { get; set; }
    }
}
