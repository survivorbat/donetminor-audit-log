using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Minor.Miffy.MicroServices.Events;

namespace ExampleService
{
    /// <summary>
    /// The more technical and logic part of the SeedData
    ///
    /// This class reflects the Calling Assembly and takes all the DomainEvents
    /// it can find. Then it'll evaluate if there is any seeddata defined for those
    /// classes and then packs them together and sends them back.
    ///
    /// PLEASE NOTE THAT THIS CLASS HAS NOTHING TO DO WITH THE AUDITLOGGER AND
    /// IS JUST AN OVER-ENGINEERED EXAMPLE
    /// </summary>
    internal static partial class ExampleData
    {
        /// <summary>
        /// A random object to enable us to choose random events
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// All the event types in the current context
        /// </summary>
        private static readonly IList<GeneratableType> AvailableEventTypes = new List<GeneratableType>();

        /// <summary>
        /// Available dummy data properties
        /// </summary>
        private static readonly IList<string> DummyDataProperties;

        /// <summary>
        /// The type that represents a base-class of a domain event type
        /// </summary>
        private static readonly Type DomainEventType = typeof(DomainEvent);

        /// <summary>
        /// Seed data type
        /// </summary>
        private static readonly Type SeedDataType = typeof(ExampleData);

        /// <summary>
        /// A domainevent type that can be generated
        /// </summary>
        private class GeneratableType
        {
            public Type Type { get; set; }
            public IList<PropertyInfo> PropertyInfos { get; } = new List<PropertyInfo>();
        }

        /// <summary>
        /// Select all types from the current assembly and add them to the list
        /// of domain types
        /// </summary>
        static ExampleData()
        {
            DummyDataProperties = SeedDataType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(e => e.Name)
                .ToArray();

            Assembly assembly = Assembly.GetCallingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsSubclassOf(DomainEventType))
                {
                    continue;
                }

                RegisterGeneratableType(type);
            }
        }

        /// <summary>
        /// Register a type that can be generated in this seed data
        /// </summary>
        private static void RegisterGeneratableType(Type type)
        {
            GeneratableType generatableType = new GeneratableType
            {
                Type = type
            };

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!DummyDataProperties.Contains($"{propertyInfo.Name}s"))
                {
                    continue;
                }

                generatableType.PropertyInfos.Add(propertyInfo);
            }

            AvailableEventTypes.Add(generatableType);
        }

        /// <summary>
        /// Generate a random event type with populated data
        /// </summary>
        public static DomainEvent GenerateRandomEvent()
        {
            if (!AvailableEventTypes.Any())
            {
                throw new ArgumentException("No available event types found to generate");
            }

            GeneratableType type = AvailableEventTypes[Random.Next(0, AvailableEventTypes.Count)];
            object instance = Activator.CreateInstance(type.Type);

            foreach (PropertyInfo propertyInfo in type.PropertyInfos)
            {
                var dummyDataProperty = SeedDataType.GetField($"{propertyInfo.Name}s")
                    ?.GetValue(null) as object[];

                propertyInfo.SetValue(instance, dummyDataProperty?[Random.Next(dummyDataProperty.Length)]);
            }

            return instance as DomainEvent;
        }
    }
}
