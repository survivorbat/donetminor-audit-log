using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalDeletedEvent : DomainEvent
    {
        public AnimalDeletedEvent() : base("App.Animals.Delete")
        {
        }

        public Animal Animal { get; set; }
    }
}
