using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class PersonAddedEvent : DomainEvent
    {
        public PersonAddedEvent() : base("App.Persons.PersonAdded")
        {
        }

        public Person Person { get; set; }
    }
}
