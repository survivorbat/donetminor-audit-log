using ExampleService.Model;

namespace ExampleService
{
    /// <summary>
    /// A class with example seed data to generate events.
    ///
    /// The name of the fields have to correspond to the names of the properties in the event.
    /// </summary>
    internal static partial class ExampleData
    {
        /// <summary>
        /// Example persons
        /// </summary>
        public static readonly Person[] Persons = {
            new Person {FirstName = "Jan", LastName = "Hendrik", Age = 23},
            new Person {FirstName = "Peter", LastName = "Vickan", Age = 34},
            new Person {FirstName = "Bernt", LastName = "Lennon", Age = 44},
        };

        /// <summary>
        /// Example Animals
        /// </summary>
        public static readonly Animal[] Animals = {
            new Cat("Jeffrey") {FavouriteCatFood = "Gourmet", FurColor = "Gray"},
            new Cat("Aspra") {FavouriteCatFood = "Pura", FurColor = "White"},
            new Cat("Luna") {FavouriteCatFood = "Whiskas", FurColor = "White"},
            new Elephant("John") { SkinColor = "Gray"},
            new Elephant("Patrick") { SkinColor = "Brown"},
        };
    }
}
