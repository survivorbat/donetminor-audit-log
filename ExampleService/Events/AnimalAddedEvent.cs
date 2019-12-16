using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalAddedEvent : DomainEvent
    {
        public AnimalAddedEvent() : base(TopicNames.AnimalAddedTopic)
        {
        }

        public Animal Animal { get; set; }
    }
}
