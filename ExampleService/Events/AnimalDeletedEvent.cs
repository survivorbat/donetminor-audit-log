using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalDeletedEvent : DomainEvent
    {
        public AnimalDeletedEvent() : base(TopicNames.AnimalDeletedTopic)
        {
        }

        public Animal Animal { get; set; }
    }
}
