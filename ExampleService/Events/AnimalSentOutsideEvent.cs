using ExampleService.Constants;
using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class AnimalSentOutsideEvent : DomainEvent
    {
        public AnimalSentOutsideEvent() : base("App.Animals.SendOutside")
        {
        }

        public Animal Animal { get; set; }
    }
}
