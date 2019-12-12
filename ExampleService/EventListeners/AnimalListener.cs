using ExampleService.Events;
using MaartenH.Minor.Miffy.AuditLogging.Events;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.EventListeners
{
    public class AnimalListener
    {
        [EventListener("App.Animals.AnimalAdded")]
        public void HandleAnimalAdded(AnimalAddedEvent evt)
        {
        }

        [ReplayEventListener("App.Animals.AnimalAdded")]
        public void HandleAnimalAddedReplay(AnimalAddedEvent evt)
        {
        }
    }
}
