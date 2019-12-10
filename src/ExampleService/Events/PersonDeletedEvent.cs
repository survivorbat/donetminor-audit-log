using ExampleService.Model;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.Events
{
    public class PersonDeletedEvent : DomainEvent
    {
        public PersonDeletedEvent() : base("App.Persons.PersonDeleted")
        {
        }

        public Person Person { get; set; }
    }
}
