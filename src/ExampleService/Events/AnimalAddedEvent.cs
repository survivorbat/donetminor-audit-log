using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalAddedEvent : DomainEvent
    {
        public AnimalAddedEvent() : base("App.Animals.AnimalAdded")
        {
        }

        public Animal Animal { get; set; }
    }
}
