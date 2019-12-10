using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalSentInsideEvent : DomainEvent
    {
        public AnimalSentInsideEvent() : base("App.Animals.SendInside")
        {
        }

        public Animal Animal { get; set; }
    }
}
