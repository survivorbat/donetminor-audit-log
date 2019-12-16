using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalSentInsideEvent : DomainEvent
    {
        public AnimalSentInsideEvent() : base(TopicNames.AnimalSentInsideTopic)
        {
        }

        public Animal Animal { get; set; }
    }
}
