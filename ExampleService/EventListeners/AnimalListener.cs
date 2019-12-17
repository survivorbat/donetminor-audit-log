using System;
using ExampleService.Constants;
using ExampleService.Events;
using MaartenH.Minor.Miffy.AuditLogging.Events;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService.EventListeners
{
    public class AnimalListener
    {
        [EventListener(QueueNames.AnimalAddedQueue)]
        [Topic(TopicNames.AnimalAddedTopic)]
        public void HandleAnimalAdded(AnimalAddedEvent evt)
        {
            Console.WriteLine("Received a AnimalAddedEvent!");
        }

        [ReplayEventListener(QueueNames.AnimalAddedQueue)]
        public void HandleAnimalAddedReplay(AnimalAddedEvent evt)
        {
            Console.WriteLine("Received a replayed AnimalAddedEvent!");
        }
    }
}
